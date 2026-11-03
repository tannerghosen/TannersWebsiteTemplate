namespace TannersWebsiteTemplate.Middleware
{
    // https://learn.microsoft.com/en-us/aspnet/core/fundamentals/middleware/extensibility?view=aspnetcore-10.0
    public class SessionMiddleware : IMiddleware
    {
        private SessionManager _s;
        public SessionMiddleware(SessionManager s)
        {
            _s = s;
        }

        // This simply creates a new session if the user was logged in and it expired / doesn't exist
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            await _s.CreateNewSession();

            // Otherwise continue the request as normal
            await next(context);
        }
    }
}
