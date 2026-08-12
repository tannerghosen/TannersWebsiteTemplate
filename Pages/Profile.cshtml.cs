using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TannersWebsiteTemplate.Pages
{
    public class ProfileModel : PageModel
    {
        [BindProperty]
        public int Id { get; set; }

        [BindProperty]
        public string? Username { get; set; }

        [BindProperty]
        public DateTime? JoinDate { get; set; }

        [BindProperty]
        public int? TotalComments { get; set; }

        [BindProperty]
        public string? AccountType { get; set; }
        public async Task OnGet()
        {
            //Id = Convert.ToInt32(Request.Query["UserId"]);
            Id = Convert.ToInt32(RouteData.Values["id"]);
            Username = await SQL.Accounts.GetUsername(Id) == null || await SQL.Accounts.GetUsername(Id) == string.Empty ? "Not Registered" : await SQL.Accounts.GetUsername(Id);
            AccountType = await SQL.Admin.IsAdmin(Id) == true ? Id == 1 ? "Owner" : "Admin" : Id == -1 ? "Guest" : "Member";
            JoinDate = await SQL.Accounts.GetJoinDate(Id) == null || await SQL.Accounts.GetJoinDate(Id) < DateTime.MinValue ? DateTime.Now : await SQL.Accounts.GetJoinDate(Id);
            TotalComments = SQL.Comments.CountCommentsByUserId(Id);
        }
    }
}
