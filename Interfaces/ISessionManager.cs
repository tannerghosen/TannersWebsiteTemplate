namespace TannersWebsiteTemplate.Interfaces
{
    public interface ISessionManager
    {
        public abstract Task Login(string username, int id, string sessionid);
        public abstract Task Logout();
        public Guid SID();
        public bool IsUserLoggedIn();
        public string GetIP();
        public Session GetSession();
    }
}
