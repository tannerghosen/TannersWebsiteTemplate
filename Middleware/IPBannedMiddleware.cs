// https://www.c-sharpcorner.com/article/middlewares-in-asp-net-core-the-ultimate-detailed-guide/
namespace TannersWebsiteTemplate.Middleware
{
    public class IPBannedMiddleware
    {
        private readonly RequestDelegate _next;

        public IPBannedMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        // To explain that what happens is when a user connects to the website, this is ran in the middleware
        // If the client is IP banned, instead of accomplishing the request we redirect to IPBanned unless they're already there (to prevent infinite redirect loops)
        // If the client isn't IP banned, we let the request continue as normal
        public async Task InvokeAsync(HttpContext context)
        {
            var ip = context.Connection.RemoteIpAddress?.ToString(); // the IP of the client
            var path = context.Request.Path; // their http request's path
            
            // If we're on IPBanned, do nothing. This is here to prevent infinite redirect loops from the below if statement.
            if (path.Equals("/IPBanned", StringComparison.OrdinalIgnoreCase))
            {
                await _next(context);
                return;
            }

            // If this IP is banned, always redirect to the IPBanned page
            if (SQL.Admin.IsUserIPBannedSimple(ip))
            {
                context.Response.Redirect("/IPBanned");
                return;
            }

            // Otherwise continue the request as normal
            await _next(context);
        }
    }
}
