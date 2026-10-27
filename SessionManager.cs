using Microsoft.AspNetCore.Authentication;
using TannersWebsiteTemplate;
using TannersWebsiteTemplate.Interfaces;

public struct Session
{
    public string? Username;
    public int? UserId;
    public string? SessionId;
    public int? IsLoggedIn;
    public int? IsAdmin;
}
public class SessionManager : ISessionManager
{

    private IHttpContextAccessor _h;

    public SessionManager(IHttpContextAccessor h)
    {
        _h = h;
    }
    public async Task Login(string username, int id, string sessionid)
    {
        _h.HttpContext?.Session.SetString("Username", username);
        _h.HttpContext?.Session.SetInt32("UserId", await TannersWebsiteTemplate.SQL.Accounts.GetUserID(username));
        _h.HttpContext?.Session.SetString("SessionId", sessionid);
        _h.HttpContext?.Session.SetInt32("IsLoggedIn", 1);
        _h.HttpContext?.Session.SetInt32("IsAdmin", await TannersWebsiteTemplate.SQL.Admin.IsAdmin(_h.HttpContext.Session.GetInt32("UserId")) == true ? 1 : 0);
        await Logger.Write("Username: " + username + " id: " + await TannersWebsiteTemplate.SQL.Accounts.GetUserID(username) + " session id " + sessionid + " is admin?: " + await TannersWebsiteTemplate.SQL.Admin.IsAdmin(_h.HttpContext.Session.GetInt32("UserId")), "LOGIN");
    }

    public async Task Logout()
    {
        if (IsUserLoggedIn() && (_h.HttpContext?.Session.GetString("Username") != null || _h.HttpContext?.Session.GetString("Username") != ""))
        {
            await Logger.Write("Username: " + _h.HttpContext.Session.GetString("Username") + " id: " + _h.HttpContext.Session.GetInt32("UserId") + " session id: " + _h.HttpContext.Session.GetString("SessionId"), "LOGOUT");
            _h.HttpContext?.Session.SetString("Username", "");
            _h.HttpContext?.Session.SetInt32("UserId", -1);
            _h.HttpContext?.Session.SetString("SessionId", "");
            _h.HttpContext?.Session.SetInt32("IsLoggedIn", 0);
            _h.HttpContext?.Session.SetInt32("IsAdmin", 0);
        }
    }

    public Guid SID()
    {
        return Guid.NewGuid();
    }

    public bool IsUserLoggedIn()
    {
        if (_h.HttpContext?.Session.GetInt32("IsLoggedIn") == 1)
        {
            return true;
        }
        return false;
    }

    public string GetIP()
    {
        return _h.HttpContext.Connection.RemoteIpAddress.ToString();
    }

    public Session GetSession()
    {
        return new Session { Username = _h.HttpContext?.Session.GetString("Username"), UserId = _h.HttpContext?.Session.GetInt32("UserId"), SessionId = _h.HttpContext?.Session.GetString("SessionId"), IsLoggedIn = _h.HttpContext?.Session.GetInt32("IsLoggedIn"), IsAdmin = _h.HttpContext?.Session.GetInt32("IsAdmin") };
    }

    public async Task LoginIfCookieExists()
    {
        // If Authenticated (always true) && Username is in the claims and it's not null && Session's username is either empty or null
        if (_h.HttpContext.User.Identity.IsAuthenticated && _h.HttpContext.User.FindFirst("Username").Value != null && string.IsNullOrEmpty(GetSession().Username))
        {
            await Logger.Write("User " + _h.HttpContext.User.FindFirst("Username").Value + " returning, logging them back in to the session...", "LOGIN");
            Login(_h.HttpContext.User.FindFirst("Username").Value, await TannersWebsiteTemplate.SQL.Accounts.GetUserID(_h.HttpContext.User.FindFirst("Username").Value), SID().ToString());
        }
    }
}