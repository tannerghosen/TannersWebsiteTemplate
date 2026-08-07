using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace TannersWebsiteTemplate
{
    public class AccountController : Controller
    {
        SessionManager _s;
        public AccountController(SessionManager s)
        {
            _s = s;
        }

        private Regex EmailRegex = Regexes.GetEmailRegex();
        private Regex UsernameRegex = Regexes.GetAccountRegex();
        public async Task<IActionResult> Login(string Username, string Password, bool External = false)
        {
            if (!_s.IsUserLoggedIn())
            {
                Guid sid = _s.SID(); // generate session id
                Username = EmailRegex.IsMatch(Username) == true ? SQL.Accounts.GetUsername(Username) : Username;
                (bool result, bool error) = (false, false);
                // If external login source
                if (External == true)
                {
                    // If it's an external login and this method is being called, it's a successful login via that site
                    // So all we need to really prove here is the user does actually exist
                    (result, error) = (SQL.Accounts.DoesUserExist(Username), false);
                }
                // Else, just login like normaal.
                else
                {
                    (result, error) = await SQL.Accounts.Login(Username, Password, sid.ToString());
                }

                // if the result from the login is true (successful outcome)
                if (result == true)
                {
                    if (!SQL.Admin.IsUserBannedSimple(SQL.Accounts.GetUserID(Username))) // if user is not banned
                    {
                        await _s.Login(Username, SQL.Accounts.GetUserID(Username), sid.ToString());
                        Statistics.IncrementLogins();
                        return Ok("Login successful. Logged in as: " + Username + ".");
                    }
                    else
                    {
                        return Ok("Banned");
                    }
                }
                else if (result == false && error != true)
                {
                    return BadRequest("Invalid login");
                }
                else if (error == true)
                {
                    return StatusCode(500, "An error occurred while logging in");
                }
            }
            return StatusCode(403, "You're already logged in.");
        }

        public async Task<IActionResult> Register(string Email, string Username, string Password, string? SecurityQuestion = null, string? Answer = null)
        {
            if(!_s.IsUserLoggedIn())
            {
                Guid sid = _s.SID(); // generate session id
                (bool result, bool error) = await SQL.Accounts.Register(Email, Username, Password, sid.ToString()); 
                if (result == true)
                {
                    await Logger.Write("Registration successful. New user added: " + Username, "REGISTER");
                    await _s.Login(Username, SQL.Accounts.GetUserID(Username), sid.ToString());
                    await SQL.Accounts.CreateSecurityQuestion(SQL.Accounts.GetUserID(Username), SecurityQuestion, Answer);
                    Statistics.IncrementRegistrations();
                    return Ok("Account Registered. Logged into " + Username + ".");
                }
                else if (result == false && error != true)
                {
                    BadRequest("Duplicate account.");
                }
                else if (error == true)
                {
                    StatusCode(500, "An error occured while registering the account.");
                }
            }
            return StatusCode(403, "You're already logged in, no need to register an account!");
        }

        public async Task<IActionResult> Logout()
        {
            string? username = _s.GetSession().Username;
            if (_s.IsUserLoggedIn() && (username != null || username != ""))
            {
                await _s.Logout();
                return Ok("Logged out of "+ username);
            }
            return BadRequest("You're not logged in.");
        }
    }
}
