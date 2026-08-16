using Microsoft.AspNetCore.Components;
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
            // If the RouteData.Values["id"] is a string (it always will be) and if the string is parseable to an int, set it to id, else set it to 0
            Id = RouteData.Values["id"] is string s == true && int.TryParse(s, out int id) == true ? Id = id : Id = 0;
            Username = await SQL.Accounts.GetUsername(Id) == null || await SQL.Accounts.GetUsername(Id) == string.Empty ? "Not Registered" : await SQL.Accounts.GetUsername(Id);
            AccountType = await SQL.Admin.IsAdmin(Id) == true ? Id == 1 ? "Owner" : "Admin" : Id == -1 ? "Guest" : "Member";
            JoinDate = await SQL.Accounts.GetJoinDate(Id) == null || await SQL.Accounts.GetJoinDate(Id) < DateTime.MinValue ? DateTime.Now : await SQL.Accounts.GetJoinDate(Id);
            TotalComments = await SQL.Comments.CountCommentsByUserId(Id);
        }
    }
}
