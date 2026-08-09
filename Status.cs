namespace TannersWebsiteTemplate
{
    public static class Status
    {
        public static string status = "";

        private static string AccessPassword = "";

        public static async Task CreateAccessPassword()
        {
            AccessPassword = PasswordHelper.GeneratePassword(); // generate a password needed to actually send new updates to the websocket server (can be get'd)
            // Remove special characters that can cause issues
            var chars = new string[] { "!", "@", "#", "$", "%", "^", "&", "*" };
            for (int i = 0; i < chars.Length; i++)
            {
                AccessPassword = AccessPassword.Replace(chars[i], "");
            }
        }
      
        public static string GetAccessPassword()
        {
            return AccessPassword;
        }
    }
}
