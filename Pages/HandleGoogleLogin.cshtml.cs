using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using TannersWebsiteTemplate.Helpers;


namespace TannersWebsiteTemplate.Pages
{
    // This is the redirect page that handles the data after we initiated the OAuth2 login challenge in Login and it goes to the CallbathPack. We redirect to WelcomeExternal.
    public class HandleGoogleLoginModel : PageModel
    {
        private readonly SessionManager _s;
        private readonly AccountController _a;
        private IHttpContextAccessor _h;

        public HandleGoogleLoginModel(SessionManager s, AccountController a, IHttpContextAccessor h)
        {
            _s = s;
            _a = a;
            _h = h;
        }
        // This method accesses the authenticated request the middleware at CallbathPack handles from the challenge we started back in Login.cshtml.cs.
        // Based on the data, we either login the user or register them, or we redirect back to login because the authentication here failed.
        // The next step is to redirect to WelcomeExternal, which either is the end of the process or it redirects to Index and stops there
        // HandleGoogleLogin -> WelcomeExternal (final stop if register, introduction, or redirect if login, all log in related stuff is finalized here)
        public async Task<IActionResult> OnGetAsync()
        {
            var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme); // Authenticate the current request using ASP.NET Core's Google auth scheme
            if (result.Succeeded) // If authenticated successfully , we can access the claims from the results
            {
                // Claims are the data that Google sends back to us that contains the authenticated user's identity
                // These can consist of the user's email, username, etc.
                var claims = result.Principal.Claims;
                // We grab the claims we need to for an account to register / log in, Username (Name) and Email
                var username = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value; // Username
                var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value; // Email

                // If the user doesn't exist, we make their account
                if (!await SQL.Accounts.DoesUserExist(email, "email"))
                {
                    if (await SQL.Accounts.DoesUserExist(username, "username"))
                    {
                        // If this user already exists, we prevent an issue by adding a number to the end of the username
                        Random random = new Random();
                        int num = random.Next(1, 9999);
                        username = username + num.ToString();
                    }
                    string password = PasswordHelper.GeneratePassword();
                    TempData["TempPassword"] = password; // we store this for WelcomeExternal's message so the user can see their password
                    await _a.RegisterExternal(email, username, password);
                }
                else // otherwise if they exist, we just log in as google has affirmed the user is who they say they are
                {
                    await _a.Login(email, "", true);
                }

                // We redirect back to WelcomeExternal, which for new users will introduce them to the sites, and for returning visitors redirect instantly back to Index.
                return RedirectToPage(Url.Page("/WelcomeExternal"));
            }
            else
            {
                await Logger.Write("Google authentication failed!", "ERROR");
                return RedirectToPage("/Login", new { Result = "Google authentication failed." });
            }
        }
    }
}
