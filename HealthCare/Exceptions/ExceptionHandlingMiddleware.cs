namespace HealthCare.Exceptions
{
    using Microsoft.AspNetCore.Http;
    using Serilog;
    using System;
    using System.Net;
    using System.Threading.Tasks;

    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An unhandled exception occurred during the request.");

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                var response = new { error = ex.Message };
                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }

}
