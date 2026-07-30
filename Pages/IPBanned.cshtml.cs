using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.RateLimiting;

namespace TannersWebsiteTemplate.Pages
{
    [DisableRateLimiting]
    public class IPBannedModel : PageModel
    {
        public void OnGet()
        {
            if (!TannersWebsiteTemplate.SQL.Admin.IsUserIPBannedSimple(HttpContext.Connection.RemoteIpAddress.ToString()))
            {
                Response.Redirect("/Index");
            }
        }
    }
}
