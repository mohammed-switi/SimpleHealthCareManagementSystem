using HealthCare.Services;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace HealthCare.Controllers
{

    [Route("api/messages")]
    [ApiController]
    public class MessageController : ControllerBase
    {

        private readonly MessageService _messageService;

        private readonly IWhatsAppApi _whatsAppApi;

        private readonly IConfiguration _configuration;



        public MessageController(MessageService messageService, IWhatsAppApi whatsAppApi, IConfiguration configuration)
        {
            _messageService = messageService;
            _whatsAppApi = whatsAppApi;
            _configuration = configuration;
        }


        [HttpPost]
        public async Task<IActionResult> sendMessage([FromBody] MessageRequest request)
        {

            if (request == null || string.IsNullOrEmpty(request.RecipientPhoneNumber))
            {
                return BadRequest("Invalid request data");
            }

            try
            {

                await _messageService.SendMessage(request.RecipientPhoneNumber);
                return Ok("Message Was Sent Successfully");
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }

        }


        [HttpPost("refit")]
        public async Task<IActionResult> SendMessageRefit([FromBody] MessageRequest request)
        {
            Log.Information("Received SendMessageRefit request: {@Request}", request);

            if (request == null || string.IsNullOrEmpty(request.RecipientPhoneNumber))
            {
                Log.Warning("Invalid request data: {@Request}", request);
                return BadRequest("Invalid request data");
            }

            try
            {
                var data = new MessagingRequest
                {
                    MessagingProduct = "whatsapp",
                    To = request.RecipientPhoneNumber,
                    Type = "template",
                    Template = new Template
                    {
                        Name = "welcome",
                        Language = new Language
                        {
                            Code = "en"
                        }
                    }
                };


                Log.Information("Sending message to WhatsApp API: {@MessagingRequest}", data);
                await _whatsAppApi.SendTemplateMessage(data, _configuration["WhatsAppSettings:AccessToken"]);
     
                Log.Information("Message sent successfully to {RecipientPhoneNumber}", request.RecipientPhoneNumber);

                return Ok("Message Was Sent Successfully");
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Error occurred while sending message to {RecipientPhoneNumber}", request.RecipientPhoneNumber);
                return BadRequest(ex.Message);
            }
        }

    


    }

    public class MessageRequest
    {
        public string RecipientPhoneNumber { get; set; }
    }
}
