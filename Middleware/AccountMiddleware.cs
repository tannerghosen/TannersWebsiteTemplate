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

        // This simply restores the session if the user was logged in and it expired. 
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            await _s.RestoreSession();

            // Otherwise continue the request as normal
            await next(context);
        }
    }
}
