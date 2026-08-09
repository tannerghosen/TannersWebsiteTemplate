using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.RegularExpressions;
using TannersWebsiteTemplate.Helpers;

namespace TannersWebsiteTemplate.Pages
{
    public class SettingsModel : PageModel
    {
        [BindProperty]
        public string Email { get; set; }
        [BindProperty]
        public string Username { get; set; }
        [BindProperty]
        public string Password { get; set; }

        [BindProperty]
        public string Result { get; set; }

        private Regex EmailRegex = Regexes.GetEmailRegex();
        private Regex UsernameRegex = Regexes.GetAccountRegex();
        public void OnGet()
        {
            if (HttpContext.Session.GetInt32("IsLoggedIn") != 1)
            {
                Response.Redirect("/Index");
            }
        }

        public async Task<IActionResult> OnPost()
        {
            if (HttpContext.Session.GetInt32("IsLoggedIn") == 1 && HttpContext.Session.GetString("Username") != null && HttpContext.Session.GetInt32("UserId") != null && HttpContext.Session.GetInt32("SessionId") != null)
            {
                if (!string.IsNullOrEmpty(Password))
                {
                    // for these if-elses with xupdated, the expected outcome is either it updates it or not.
                    // because the error could
                    (bool passwordupdated, bool error) = await SQL.Accounts.UpdateInfo(HttpContext.Session.GetInt32("UserId"), 0, Password, HttpContext.Session.GetString("SessionId"));
                    if (passwordupdated)
                    {
                        Result += "Password has been changed. Be sure to write it down or save it in your browser!"; // Success
                    }
                    else if (!passwordupdated && !error)
                    {
                        Result += "An unknown error occurred while changing the Password."; // unlike below where dups can happen, this should never happen
                    }
                    else if (!passwordupdated && error)
                    {
                        Result += "An error occurred while changing the Password."; // SQL Error
                    }
                }
                else if (string.IsNullOrEmpty(Password))
                {
                }

                if (!string.IsNullOrEmpty(Email) && EmailRegex.IsMatch(Email))
                {
                    (bool emailupdated, bool error) = await SQL.Accounts.UpdateInfo(HttpContext.Session.GetInt32("UserId"), 1, Email, HttpContext.Session.GetString("SessionId"));
                    if (emailupdated)
                    {
                        Result += "\\nEmail has been updated to " + Email; // Success
                    }
                    else if (!emailupdated && !error)
                    {
                        Result += "\\nEmail is already in use by another account!"; // SQL Conflict (Email used by another account)
                    }
                    else if (!emailupdated && error)
                    {
                        Result += "\\nAn error occurred while changing the Email."; // Error / SQL Error
                    }
                }
                else if (string.IsNullOrEmpty(Email))
                { 
                }
                else if (!EmailRegex.IsMatch(Email))
                {
                    Result += "\\nEmail format is invalid. Emails should follow a format of name@emailprovider.com";
                }

                if (!string.IsNullOrEmpty(Username))
                {
                    if (UsernameRegex.IsMatch(Username))
                    {
                        (bool usernameupdated, bool error) = await SQL.Accounts.UpdateInfo(HttpContext.Session.GetInt32("UserId"), 2, Username, HttpContext.Session.GetString("SessionId"));
                        if (usernameupdated)
                        {
                            HttpContext.Session.SetString("Username", Username);
                            Result += "\\nUsername updated to " + Username; // Success
                        }
                        else if (!usernameupdated && !error)
                        {
                            Result += "\\nUsername is already in use by another account!"; // SQL Conflict (Username used by another account)
                        }
                        else if (!usernameupdated && error)
                        {
                            Result += "\\nAn error occurred while changing the Username."; // Error / SQL Error
                        }
                    }
                    else
                    {
                        Result += "\\nInvalid username.";
                    }
                }
                else if (string.IsNullOrEmpty(Username))
                {
                }
            }
            TempData["Result"] = Result;

            return Page();
        }
    }
}
