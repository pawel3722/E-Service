using EService.Dtos.MessageDtos;
using EService.Dtos.ServiceTypeDtos;
using EService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessageController(IMessageService service)
        {
            _messageService = service;
        }

        [HttpGet("admin/{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _messageService.GetMessageAsync(id);
            if (result != null)
                return Ok(result);
            return NotFound();
        }

        [HttpGet("admin"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> Get()
        {
            var result = await _messageService.GetAllMessagesAsync();
            if (result != null)
                return Ok(result);
            return NotFound();
        }

        [HttpGet("sender"), Authorize]
        public async Task<IActionResult> GetSentMessages([FromQuery]int? receiverId)
        {
            var result = await _messageService.GetSentMessagesAsync(receiverId);
            if (result.Confirmed)
                if (result.Messages != null)
                    return Ok(result.Messages);
                else return StatusCode(500, result.Response);
            else return BadRequest(result.Response);
        }

        [HttpGet("receiver"), Authorize]
        public async Task<IActionResult> GetReceivedMessages([FromQuery]int? senderId)
        {
            var result = await _messageService.GetReceivedMessagesAsync(senderId);
            if (result.Confirmed)
                if (result.Messages != null)
                    return Ok(result.Messages);
                else return StatusCode(500, result.Response);
            else return BadRequest(result.Response);
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> Create(CreateMessageDto request)
        {
            var result = await _messageService.CreateMessageAsync(request);
            if (result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);
        }

        [HttpPut("admin/{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(UpdateMessageDto request, int id)
        {
            var result = await _messageService.UpdateMessageAsync(request, id);
            if (result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);
        }

        [HttpPut("{id}"), Authorize]
        public async Task<IActionResult> UpdateSentMessage(UpdateMessageDto request, int id)
        {
            var result = await _messageService.UpdateSentMessageAsync(request, id);
            if (result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);
        }

        [HttpDelete("admin/{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _messageService.DeleteMessageAsync(id);
            if (result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);
        }

        [HttpDelete("{id}"), Authorize]
        public async Task<IActionResult> DeleteSentMessage(int id)
        {
            var result = await _messageService.DeleteSentMessageAsync(id);
            if (result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);
        }
    }
}
