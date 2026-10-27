namespace TannersWebsiteTemplate.Middleware
{
    // https://learn.microsoft.com/en-us/aspnet/core/fundamentals/middleware/extensibility?view=aspnetcore-10.0
    public class AccountMiddleware : IMiddleware
    {
        private SessionManager _s;
        public AccountMiddleware(SessionManager s)
        {
            _s = s;
        }

        // This simply logs the user into the session if they have a 
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            await _s.LoginIfCookieExists();

            // Otherwise continue the request as normal
            await next(context);
        }
    }
}
