using System.Text.RegularExpressions;

namespace TannersWebsiteTemplate
{
    public static class Regexes
    {
        private static Regex EmailRegex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
        private static Regex AccountRegex = new Regex(@"^(?!\s)(?!.*[\W_]{2,})[a-zA-Z0-9_\s]+$");
        private static Regex PasswordRegex = new Regex(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-])(?!.*(.)\\1{5,}).{8,32}$");

        public static Regex GetEmailRegex()
        {
            return EmailRegex;
        }

        public static Regex GetAccountRegex()
        {
            return AccountRegex;
        }

        public static Regex GetPasswordRegex()
        {
            return PasswordRegex;
        }
    }
}
