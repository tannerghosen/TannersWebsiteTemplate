using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TannersWebsiteTemplate.Pages
{
    public class LogoutModel : PageModel
    {
        private SessionManager _s;
        private AccountController _a;
        public LogoutModel(SessionManager s, AccountController a)
        {
            _s = s;
            _a = a;
        }
        public void OnGet()
        {
            if (HttpContext.Session.GetInt32("IsLoggedIn") != 1)
            {
                Response.Redirect("/Index");
            }
        }
        public async Task<IActionResult> OnPost()
        {
            await _a.Logout();;

            return Redirect("/Index");
        }
    }
}
