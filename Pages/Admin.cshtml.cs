using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.NetworkInformation;
using TannersWebsiteTemplate.Models;

namespace TannersWebsiteTemplate.Pages
{
    public class AdminModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        [BindProperty]
        public string[]?[]? AccountsTable { get; set; }

        [BindProperty]
        public Stats stats { get; set; }

        public AdminModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }
        public void OnGet()
        {
            // Accounts / Security Question Tables joined as a 2d array (row, column). each user entry is 1 row, with multiple columns
            AccountsTable = SQL.Admin.GrabAccountsTable();
            // row[5] being modified from being a boolean true / false to a Yes / No via a select statement
            AccountsTable = AccountsTable.Select(row =>
            {
                if (Convert.ToBoolean(row[5]) == true) // if the result of column 5 (Are they Admin?) is true, set it to Yes, else No
                {
                    row[5] = "Yes";
                }
                else
                {
                    row[5] = "No";
                }
                return row;
            }).ToArray();
            if (HttpContext.Session.GetInt32("IsAdmin") != 1 || !SQL.Admin.IsAdmin(HttpContext.Session.GetInt32("UserId")))
            {
                Response.Redirect("/Index");
            }
            stats = Statistics.GetStats();
        }

        public async Task<IActionResult> OnPostDelete(int? userid)
        {
            if (HttpContext.Session.GetInt32("IsAdmin") == 1 && SQL.Admin.IsAdmin(HttpContext.Session.GetInt32("UserId"))) // is the user admin and does the userid in session check out as admin? if so, delete user requested.
            {
                await SQL.Admin.DeleteUser(userid);
            }

            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostAdmin(int? userid)
        {
            if (HttpContext.Session.GetInt32("IsAdmin") == 1 && HttpContext.Session.GetInt32("UserId") == 1 && SQL.Admin.IsAdmin(HttpContext.Session.GetInt32("UserId"))) // is the user admin and is it super admin doing this and does the userid in session check out as admin? if so, make the requested userid an admin
            {
                await SQL.Admin.AdminUser(userid);
            }

            return RedirectToPage();
        }

        public IActionResult OnPostResetStats()
        {
            if (HttpContext.Session.GetInt32("IsAdmin") == 1 && HttpContext.Session.GetInt32("UserId") == 1 && SQL.Admin.IsAdmin(HttpContext.Session.GetInt32("UserId"))) // is the user admin and is it super admin doing this and does the userid in session check out as admin? if so, make the requested userid an admin
            {
                Statistics.ResetStats();
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostUnbanUser(int? userid)
        {
            if (HttpContext.Session.GetInt32("IsAdmin") == 1 && HttpContext.Session.GetInt32("UserId") == 1 && SQL.Admin.IsAdmin(HttpContext.Session.GetInt32("UserId"))) // is the user admin and is it super admin doing this and does the userid in session check out as admin? if so, make the requested userid an admin
            {
                await SQL.Admin.UnbanUser(userid);
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostBanUser(int? userid)
        {
            if (HttpContext.Session.GetInt32("IsAdmin") == 1 && HttpContext.Session.GetInt32("UserId") == 1 && SQL.Admin.IsAdmin(HttpContext.Session.GetInt32("UserId"))) // is the user admin and is it super admin doing this and does the userid in session check out as admin? if so, make the requested userid an admin
            {
                await SQL.Admin.BanUser(userid, "You have been banned.", DateTime.Now.AddYears(999));
            }
            return RedirectToPage();
        }
    }
}
