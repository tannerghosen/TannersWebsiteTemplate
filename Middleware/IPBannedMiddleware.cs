// https://www.c-sharpcorner.com/article/middlewares-in-asp-net-core-the-ultimate-detailed-guide/
using TannersWebsiteTemplate.SQL;
namespace TannersWebsiteTemplate.Middleware
{
    public class IPBannedMiddleware
    {
        private readonly RequestDelegate _next;

        public IPBannedMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var ip = context.Connection.RemoteIpAddress?.ToString(); // the IP of the client
            var path = context.Request.Path; // their http request's path
            
            // If we're on IPBanned, do nothing.
            if (path.Equals("/IPBanned", StringComparison.OrdinalIgnoreCase))
            {
                await _next(context);
                return;
            }
            if (SQL.Admin.IsUserIPBannedSimple(ip))
            {
                context.Response.Redirect("/IPBanned");
                return;
            }

            await _next(context);
        }
    }
}
