using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Serilog;

namespace HealthCare.Logging
{
    public class SerilogLoggingHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Log the request details
            var requestContent = request.Content != null ? await request.Content.ReadAsStringAsync() : string.Empty;
            Log.Information("Sending request to {Url} with method {Method} \nHeaders: {Headers}\nBody: {Body}",
                request.RequestUri, request.Method, request.Headers.ToString(), requestContent.ToString());

            // Send the request
            var response = await base.SendAsync(request, cancellationToken);

            // Log the response details
            var responseContent = response.Content != null ? await response.Content.ReadAsStringAsync() : string.Empty;
          
            Log.Information("Received response from {Url} with status code {StatusCode} \nHeaders: {Headers}\nBody: {Body}",
                response.RequestMessage.RequestUri, response.StatusCode, response.Headers.ToString(), responseContent);

            return response;
        }
    }
}
