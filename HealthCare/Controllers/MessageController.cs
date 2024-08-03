using HealthCare.Services;
using Microsoft.AspNetCore.Mvc;

namespace HealthCare.Controllers
{

    [Route("api/messages")]
    [ApiController]
    public class MessageController : ControllerBase
    {

        private readonly MessageService _messageService;


        public MessageController(MessageService messageService)
        {
            _messageService = messageService;
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

    }

    public class MessageRequest
    {
        public string RecipientPhoneNumber { get; set; }
    }
}
