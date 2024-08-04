using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.IO;
using System.Threading.Tasks;

namespace HealthCare.Middlewares;
public class LoggingMiddleware
{
    private readonly RequestDelegate _next;

    public LoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Log the request
        Log.Information("Handling request: {Method} {Url} from {IpAddress}",
            context.Request.Method,
            context.Request.Path,
            context.Connection.RemoteIpAddress);

        // Copy the original response body stream
        var originalBodyStream = context.Response.Body;

        using (var responseBody = new MemoryStream())
        {
            context.Response.Body = responseBody;

            try
            {
                await _next(context);

                context.Response.Body.Seek(0, SeekOrigin.Begin);
                var responseText = await new StreamReader(context.Response.Body).ReadToEndAsync();
                context.Response.Body.Seek(0, SeekOrigin.Begin);

                // Log the response
                Log.Information("Response: {StatusCode} {ResponseText}", context.Response.StatusCode, responseText);

                await responseBody.CopyToAsync(originalBodyStream);
            }
            catch (Exception ex)
            {
                // Log the exception
                Log.Error(ex, "An unhandled exception has occurred while executing the request.");

                // Re-throw the exception to let the default exception handler handle it
                throw;
            }
        }
    }
}
