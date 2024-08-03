using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using HealthCare.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace HealthCare.Services
{
    public class MessageService
    {
        private readonly HttpClient client = new HttpClient();
        private readonly string url = "https://graph.facebook.com/v20.0/376409038895153/messages";
        private readonly string _accessToken;

        public MessageService(IOptions<WhatsAppSettings> settings)
        {
            _accessToken = settings.Value.AccessToken;
        }

        public async Task SendMessage(string recipientPhoneNumber)
        {
            var data = new
            {
                messaging_product = "whatsapp",
                to = recipientPhoneNumber,
                type = "template",
                template = new
                {
                    name = "welcome",
                    language = new
                    {
                        code = "en"
                    }
                }
            };

            string json = JsonConvert.SerializeObject(data);
            StringContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

            try
            {
                HttpResponseMessage response = await client.PostAsync(url, content);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseBody);
            }
            catch (HttpRequestException e)
            {
                throw e;
            }
        }

    }
}
