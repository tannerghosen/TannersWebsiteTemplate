using System.Text.RegularExpressions;

namespace TannersWebsiteTemplate.Interfaces.SQL
{
    public interface IAccounts
    {
        private static Regex EmailRegex;
        private static Regex UsernameRegex;
        public static abstract Task<(bool, bool)> Register(string email, string username, string password, string sessionid = "");
        public static abstract Task<(bool, bool)> Login(string username, string password, string sessionid = "");
        public static abstract Task<(bool, bool)> UpdateInfo(int? userid, int option, string input, string? sessionid = "", bool adminupdate = false);
        public static abstract Task<bool> DoesUserExist(string value, string search = "username");
        public static abstract Task<bool> DoesUserExist(int? userid);
        public static abstract Task<int> GetUserID(string username);
        public static abstract Task<string?> GetUsername(int userid);
        public static abstract Task<string?> GetUsername(string email);
        public static abstract Task<DateTime?> GetJoinDate(int userid);
        public static abstract Task<bool> DoesSIDMatch(int? userid, string sid);
    }
}
