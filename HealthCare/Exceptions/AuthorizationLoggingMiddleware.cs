namespace HealthCare.Exceptions
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Serilog;
    using System.Threading.Tasks;

    public class AuthorizationLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthorizationLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);

            if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
            {
                Log.Warning("Forbidden access attempt to: {Path}", context.Request.Path);
            }
        }
    }

}
