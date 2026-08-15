namespace TannersWebsiteTemplate.Interfaces.SQL
{
    public interface IAdmin
    {
        public static abstract Task<string[]?[]?> GrabAccountsTable();
        public static abstract Task<bool> IsAdmin(int? userid);
        public static abstract Task DeleteUser(int? userid);
        public static abstract Task AdminUser(int? userid);
        public static abstract Task BanIP(string ip, string? reason, DateTime? expire);
        public static abstract Task<(bool, string?, string?, DateTime?)> IsUserIPBanned(string ip);
        public static abstract Task<bool> IsUserIPBannedSimple(string ip);
        public static abstract Task BanUser(int? id, string? reason, DateTime? expire);
        public static abstract Task<(bool, int?, string?, DateTime?)> IsUserBanned(int? id);
        public static abstract Task<bool> IsUserBannedSimple(int id);
        public static abstract Task UnbanUser(int? id);
        public static abstract Task UnbanIP(string ip);
    }
}
