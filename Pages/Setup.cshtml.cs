using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TannersWebsiteTemplate.Pages
{
    public class SetupModel : PageModel
    {
        [BindProperty]
        public string SetupPassword { get; set; }
        [BindProperty]
        public string Password { get; set; }
        public void OnGet()
        {
            if (Globals.FirstTimeRunning == false)
            {
                Response.Redirect("/Index");
            }
            Password = Globals.AdminPassword;
            if (TempData["SetupFinished"] == "true")
            {
                Globals.AdminPassword = "";
            }
        }
        public async Task<IActionResult> OnPost()
        {
            if (!SetupPassword.Equals(Globals.SetupPassword))
            {
                TempData["SetupFinished"] = "false";
                TempData["Result"] = "Invalid setup password";
                return Page();
            }
            else
            {
                TempData["SetupFinished"] = "true";
                Password = Globals.AdminPassword;
                Globals.FirstTimeRunning = false;
                return Page();
            }
        }
    }
}
