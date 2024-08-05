namespace HealthCare.Middlewares
{
    using Microsoft.AspNetCore.Http;
    using Serilog;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using Serilog;

    using Microsoft.AspNetCore.Http;
    using Serilog;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using Org.BouncyCastle.Asn1.Ocsp;
    using System.ServiceModel.Channels;

    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestResponseLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            // Log the request
            await LogRequest(context);

            // Copy a pointer to the original response body stream
            var originalBodyStream = context.Response.Body;

            // Create a new memory stream to hold the response
            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                // Continue down the Middleware pipeline, eventually returning to this class
                await _next(context);

                // Log the response
                await LogResponse(context);

                // Copy the contents of the new memory stream (which contains the response) to the original stream
                await responseBody.CopyToAsync(originalBodyStream);
            }
        }

        private async Task LogRequest(HttpContext context)
        {
            // Read the request body
            context.Request.EnableBuffering();
            var buffer = new byte[Convert.ToInt32(context.Request.ContentLength)];
            await context.Request.Body.ReadAsync(buffer, 0, buffer.Length);
            var bodyAsText = Encoding.UTF8.GetString(buffer);
            context.Request.Body.Position = 0;

            // Format headers
            var headers = new StringBuilder();
            foreach (var header in context.Request.Headers)
            {
                headers.AppendLine($"{header.Key}: {string.Join(", ", header.Value)}");
            }

            // Log the request details
            Log.Information("Sending request to {Url} with method {Method}\nHeaders: {Headers}\nBody: {Body}",
                context.Request.Path, context.Request.Method, headers.ToString(), bodyAsText);
        }

        private async Task LogResponse(HttpContext context)
        {
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var text = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            var headers = new StringBuilder();
            foreach (var header in context.Response.Headers)
            {
                headers.AppendLine($"{header.Key}: {string.Join(", ", header.Value)}");
            }

            Log.Information("Received response with status code {StatusCode}\nHeaders: {Headers}\nBody: {Body}",
                context.Response.StatusCode, headers.ToString(), text);
        }



    }
}