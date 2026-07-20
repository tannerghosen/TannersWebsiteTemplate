using TannersWebsiteTemplate.Controllers.Classes;

namespace TannersWebsiteTemplate
{
    public static class Status
    {
        public static string status = "";

        private static string AccessPassword { get; set; }

        public static async Task CreateAccessPassword()
        {
            AccessPassword = Password.GeneratePassword(); // generate a password needed to actually send new updates to the websocket server (can be get'd)
        }
      
        public static string GetAccessPassword()
        {
            return AccessPassword;
        }
    }
}
