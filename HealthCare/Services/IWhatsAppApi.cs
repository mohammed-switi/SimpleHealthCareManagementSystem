using Refit;
using System.Text.Json.Serialization;


namespace HealthCare.Services
{

    [Headers("accept: application/json", "Authorization: Bearer")]

    public interface IWhatsAppApi
    {
        [Headers("Content-Type: application/json;charset=utf-8")]

        [Post("/v20.0/376409038895153/messages")]
        Task<ResponseBody> SendTemplateMessage([Body] MessagingRequest body, [Header("Authorization: Bearer")] string authorization);
    }

    public class ResponseBody
    {
        [JsonPropertyName("status_code")]
        public int StatusCode { get; set; }

        [JsonPropertyName("status_message")]
        public string StatusMessage { get; set; }
    }


    public class MessagingRequest
    {
        [JsonPropertyName("messaging_product")]
        public string MessagingProduct { get; set; }

        [JsonPropertyName("to")]
        public string To { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("template")]
        public Template Template { get; set; }
    }

    public class Template
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("language")]
        public Language Language { get; set; }
    }

    public class Language
    {
        [JsonPropertyName("code")]
        public string Code { get; set; }
    }

}
