namespace TannersWebsiteTemplate.Middleware
{
    // Setup Middleware - When in setup, only allows the Setup page to be accessible
    public class SetupMiddleware
    {
        private readonly RequestDelegate _next;

        public SetupMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path; // their http request's path

            if (path.Equals("/Setup", StringComparison.OrdinalIgnoreCase))
            {
                await _next(context);
                return;
            }

            if (Globals.FirstTimeRunning == true)
            {
                context.Response.Redirect("/Setup");
                return;
            }

            // Otherwise continue the request as normal
            await _next(context);
        }
    }
}
