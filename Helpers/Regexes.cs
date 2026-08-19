using System.Text.RegularExpressions;

namespace TannersWebsiteTemplate.Helpers
{
    public static class Regexes
    {
        private static Regex EmailRegex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
        private static Regex UsernameRegex = new Regex(@"^(?!\s)(?!.*[\W_]{2,})[a-zA-Z0-9_\s]+$"); // no special characters / spaces
        private static Regex PasswordRegex = new Regex(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-])(?!.*(.)\\1{5,}).{8,32}$"); // 8-32 characters in width, 1 uppercase, lowercase number, and special character, and no repeating after 5 characters.

        public static Regex GetEmailRegex()
        {
            return EmailRegex;
        }

        public static Regex GetUsernameRegex()
        {
            return UsernameRegex;
        }

        public static Regex GetPasswordRegex()
        {
            return PasswordRegex;
        }
    }
}
