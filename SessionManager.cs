using TannersWebsiteTemplate;
using Microsoft.AspNetCore.Authentication;

public struct Session
{
    public string? Username;
    public int? UserId;
    public string? SessionId;
    public int? IsLoggedIn;
    public int? IsAdmin;
}
public class SessionManager
{

    private IHttpContextAccessor _h;

    public SessionManager(IHttpContextAccessor h)
    {
        _h = h;
    }
    public async Task Login(string username, int id, string sessionid)
    {
        _h.HttpContext.Session.SetString("Username", username);
        _h.HttpContext.Session.SetInt32("UserId", await TannersWebsiteTemplate.SQL.Accounts.GetUserID(username));
        _h.HttpContext.Session.SetString("SessionId", sessionid);
        _h.HttpContext.Session.SetInt32("IsLoggedIn", 1);
        _h.HttpContext.Session.SetInt32("IsAdmin", await TannersWebsiteTemplate.SQL.Admin.IsAdmin(_h.HttpContext.Session.GetInt32("UserId")) == true ? 1 : 0);
    }

    public async Task Logout()
    {
        if (IsUserLoggedIn() && (_h.HttpContext.Session.GetString("Username") != null || _h.HttpContext.Session.GetString("Username") != ""))
        {
            _h.HttpContext.Session.SetString("Username", "");
            _h.HttpContext.Session.SetInt32("UserId", -1);
            _h.HttpContext.Session.SetString("SessionId", "");
            _h.HttpContext.Session.SetInt32("IsLoggedIn", 0);
            _h.HttpContext.Session.SetInt32("IsAdmin", 0);
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
}