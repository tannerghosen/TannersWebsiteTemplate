using System.Text.RegularExpressions;
using System.Text;

namespace TannersWebsiteTemplate.Helpers
{
    public class PasswordHelper
    {
        private static string characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*";
        private static Regex passregex = Regexes.GetPasswordRegex();

        public static string GeneratePassword()
        {
            StringBuilder password = new StringBuilder(string.Empty);
            Random r = new Random();
            for (int i = 0; i < 16; i++)
            {
                password.Append(characters[r.Next(characters.Length)]);
            }
            string GeneratedPass = password.ToString();
            return passregex.IsMatch(GeneratedPass) == true ? GeneratedPass : GeneratePassword();
        }

        public static bool ValidatePassword(string password)
        {
            return (!passregex.IsMatch(password) || password == null || password == String.Empty) ? false : true;
        }
    }
}
