using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.Layer.Specifications.Messages;
using Services.Layer;
using Services.Layer.DTOs;
using Services.Layer.Identity;

namespace DatingAppAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly IAccountService _accountService;
        public MessagesController(IAccountService accountService, IMessageService messageService)
        {
            _messageService = messageService;
            _accountService = accountService;
        }

        //[HttpGet]
        //public async Task<IActionResult> GetMessages([FromQuery] MessageSpecifications messageSpecifications)
        //{
        //    var result = await _messageService.GetMessagesForUser(messageSpecifications);
        //    return Ok(result);
        //}

        [HttpPost]
        public async Task<IActionResult> CreateMessage([FromBody] MessageDTO messageDTO)
        {
            var result = await _messageService.AddMessage(messageDTO);

            if (result.Status)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetMessagesForUser([FromQuery] MessageSpecification messageSpecifications)
        {
            messageSpecifications.UserId = _accountService.GetCurrentUserId();
            var result = await _messageService.GetMessagesForUser(messageSpecifications);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetMessagesThread([FromQuery] MessageSpecification messageSpecifications)
        {
            var result = await _messageService.GetMessagesThread(messageSpecifications);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteMessage(int messageId)
        {
            var result = await _messageService.DeleteMessage(messageId);
            return Ok(result);
        }
    }
}
