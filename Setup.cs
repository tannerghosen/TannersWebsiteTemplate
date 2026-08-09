using TannersWebsiteTemplate.Helpers;

namespace TannersWebsiteTemplate
{
    public static class Setup
    {
        public static void StartSetup()
        {
            Globals.FirstTimeRunning = true;
            string password = PasswordHelper.GeneratePassword();
            string password2 = PasswordHelper.GeneratePassword();
            Globals.AdminPassword = password;
            Globals.SetupPassword = password2;
            _ = Logger.Write("Setup password is: " + Globals.SetupPassword, "SETUP");
        }
    }
}
