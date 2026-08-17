using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace TannersWebsiteTemplate.Interfaces
{
    public interface IAccountController
    {
        private static Regex EmailRegex;
        private static Regex UsernameRegex;
        public abstract Task<IActionResult> Login(string Username, string Password, bool External = false);
        public abstract Task<IActionResult> Register(string Email, string Username, string Password, string SecurityQuestion, string Answer);
        public abstract Task<IActionResult> RegisterExternal(string Email, string Username, string Password);
        public abstract Task<IActionResult> Logout();
    }
}
