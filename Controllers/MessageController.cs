using Microsoft.AspNetCore.Mvc;
using MessageAPI.Authentication;
using MessageApi.Services;
using MessageApi.Models;

namespace MessageAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiKeyAuth]
    public class MessageController : ControllerBase
    {
        private readonly MessageService _MessageService;

        public MessageController(MessageService MessageService)
        {
            _MessageService = MessageService;
        }

        [HttpPost]
        public IActionResult InsertMessage([FromBody] Message message)
        {
            try
            {
                if (message.MsgDate == default)
                {
                    message.MsgDate = DateTime.UtcNow;
                }

                int newMsgId = _MessageService.InsertMessage(message);
                return Ok(new { MsgId = newMsgId, Message = "The message inserted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while inserting the message", Error = ex.Message });
            }
        }
    }
}