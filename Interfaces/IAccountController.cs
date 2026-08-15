using Microsoft.AspNetCore.Mvc;

namespace TannersWebsiteTemplate.Interfaces
{
    public interface IAccountController
    {
        public abstract Task<IActionResult> Login(string Username, string Password, bool External = false);
        public abstract Task<IActionResult> Register(string Email, string Username, string Password, string? SecurityQuestion = null, string? Answer = null);
        public abstract Task<IActionResult> Logout();
    }
}
