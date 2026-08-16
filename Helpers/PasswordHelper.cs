using System.Text.RegularExpressions;
using System.Text;

namespace TannersWebsiteTemplate.Helpers
{
    public class PasswordHelper
    {
        private static string Characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*";
        private static Regex PasswordRegex = Regexes.GetPasswordRegex();

        public static string GeneratePassword()
        {
            StringBuilder password = new StringBuilder(string.Empty);
            Random r = new Random();
            for (int i = 0; i < 16; i++)
            {
                password.Append(Characters[r.Next(Characters.Length)]);
            }
            string GeneratedPass = password.ToString();
            return PasswordRegex.IsMatch(GeneratedPass) == true ? GeneratedPass : GeneratePassword();
        }

        public static bool ValidatePassword(string password)
        {
            return (!PasswordRegex.IsMatch(password) || password == null || password == String.Empty) ? false : true;
        }
    }
}
