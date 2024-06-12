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

        [HttpGet("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _messageService.GetMessageAsync(id);
            if (result != null)
                return Ok(result);
            return NotFound();
        }

        [HttpGet, Authorize(Roles = "Admin")]
        public async Task<IActionResult> Get()
        {
            var result = await _messageService.GetAllMessagesAsync();
            if (result != null)
                return Ok(result);
            return NotFound();
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> Create(CreateMessageDto request)
        {
            var result = await _messageService.CreateMessageAsync(request);
            if (result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);
        }

        [HttpPut("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(UpdateMessageDto request, int id)
        {
            var result = await _messageService.UpdateMessageAsync(request, id);
            if (result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);
        }

        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _messageService.DeleteMessageAsync(id);
            if (result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);
        }
    }
}
