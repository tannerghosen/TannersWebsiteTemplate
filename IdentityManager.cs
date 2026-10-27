// https://learn.microsoft.com/en-us/aspnet/core/security/authentication/cookie?view=aspnetcore-10.0
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace TannersWebsiteTemplate
{
    public class IdentityManager
    {
        private IHttpContextAccessor _h;
        public IdentityManager(IHttpContextAccessor h)
        {
            _h = h;
        }
        public async Task Login(string username, int id)
        {

            var claims = new List<Claim>
            {
                new Claim("Username", username),
                new Claim("Id", id.ToString()),
                new Claim("IsAdmin", await SQL.Admin.IsAdmin(id) ? "1" : "0"),
            };

            var Identity = new ClaimsIdentity(claims, "TWTCookie");

            var Properties = new AuthenticationProperties
            {
                //AllowRefresh = <bool>,
                // Refreshing the authentication session should be allowed.

                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(30),
                // The time at which the authentication ticket expires. A 
                // value set here overrides the ExpireTimeSpan option of 
                // CookieAuthenticationOptions set with AddCookie.

                IsPersistent = true,
                // Whether the authentication session is persisted across 
                // multiple requests. When used with cookies, controls
                // whether the cookie's lifetime is absolute (matching the
                // lifetime of the authentication ticket) or session-based.

                //IssuedUtc = <DateTimeOffset>,
                // The time at which the authentication ticket was issued.

                //RedirectUri = <string>
                // The full path or absolute URI to be used as an http 
                // redirect response value.
            };

            await _h.HttpContext.SignInAsync("TWTCookie",new ClaimsPrincipal(Identity),Properties);
        }
        public async Task Logout()
        {
            await _h.HttpContext.SignOutAsync("TWTCookie");
        }
    }
}
