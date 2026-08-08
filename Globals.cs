namespace TannersWebsiteTemplate
{
    public struct Globals
    {
        public static string AdminPassword = ""; // Password generated for the admin account on first setup
        public static bool DisableGoogle = true; // Disable Google OAuth
        public static string DomainName = ""; // Domain Name
        public static bool FirstTimeRunning { get; set; } // First Time Running check
        public static string SetupPassword = ""; // Password to start setup
    }
}
