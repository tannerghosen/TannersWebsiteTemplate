using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TannersWebsiteTemplate.Pages
{
    public class StatusModel : PageModel
    {
        public async Task OnGet()
        {
            if ((HttpContext.Session.GetInt32("IsAdmin") != 1 || !await SQL.Admin.IsAdmin(HttpContext.Session.GetInt32("UserId"))) && HttpContext.Session.GetInt32("UserId") != 1)
            {
                Response.Redirect("/Index");
            }
        }
        public void OnPost()
        {

        }
    }
}
