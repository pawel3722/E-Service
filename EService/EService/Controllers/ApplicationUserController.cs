using EService.Dtos.MessageDtos;
using EService.Dtos.RolesDtos;
using EService.Dtos.ServiceDtos;
using EService.Services;
using EService.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUserController : ControllerBase
    {
        private readonly IApplicationUserService _applicationUserService;
        private readonly IServiceService _serviceService;
        private readonly IOrderService _orderService;

        public ApplicationUserController(IApplicationUserService applicationUserService, IServiceService serviceService, IOrderService orderService)
        { 
            _applicationUserService = applicationUserService;
            _serviceService = serviceService;
            _orderService = orderService;
        }


        [HttpGet("me/sent-messages"), Authorize]
        public async Task<IActionResult> GetSentMessages([FromQuery] int? receiverId)
        {
            var result = await _applicationUserService.GetSentMessagesAsync(receiverId);
            if (result.Confirmed)
                if (result.Messages != null)
                    return Ok(result.Messages);
                else return StatusCode(500, result.Response);
            else return BadRequest(result.Response);
        }

        [HttpGet("me/received-messages"), Authorize]
        public async Task<IActionResult> GetReceivedMessages([FromQuery] int? senderId)
        {
            var result = await _applicationUserService.GetReceivedMessagesAsync(senderId);
            if (result.Confirmed)
                if (result.Messages != null)
                    return Ok(result.Messages);
                else return StatusCode(500, result.Response);
            else return BadRequest(result.Response);
        }

        [HttpGet("me/customer-orders"), Authorize(Roles = "Client")]
        public async Task<IActionResult> GetCustomerOrders()
        {
            var result = await _applicationUserService.GetCustomerOrdersAsync();
            if (result.Confirmed)
                return Ok(result.Orders);
            else return BadRequest(result.Response);
        }

        [HttpGet("me/manager-orders"), Authorize(Roles = "Client")]
        public async Task<IActionResult> GetManagerOrders()
        {
            var result = await _applicationUserService.GetManagerOrdersAsync();
            if (result.Confirmed)
                return Ok(result.Orders);
            else return BadRequest(result.Response);
        }

        [HttpGet("me/customer-orders/services"), Authorize(Roles = "Client")]
        public async Task<IActionResult> GetClientServices(int? orderId)
        {
            var result = await _applicationUserService.GetClientServices(orderId);
            if (result.Confirmed)
                return Ok(result.Services);
            else return BadRequest(result.Response);
        }

        [HttpGet("me/customer-orders/{id}/review"), Authorize(Roles = "Client")]
        public async Task<IActionResult> GetReview(int id)
        {
            var result = await _orderService.GetReviewFromOrderAsync(id);
            if (result.Confirmed)
                return Ok(result.Review);
            else return BadRequest(result.Response);
        }

        [HttpGet("me/services"), Authorize(Roles = "Serviceman")]
        public async Task<IActionResult> GetServicemanServices(int? orderId)
        {
            var result = await _applicationUserService.GetServicemanServices(orderId);
            if (result.Confirmed)
                return Ok(result.Services);
            else return BadRequest(result.Response);
        }

        [HttpGet, Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUsers()
        {
            var result = await _applicationUserService.GetAllUsersAsync();
            if (result != null)
                return Ok(result);
            return NotFound();
        }

        [HttpGet("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUser(int id)
        {
            var result = await _applicationUserService.GetUserAsync(id);
            if (result != null)
                return Ok(result);
            return NotFound();
        }

        [HttpGet("roles/{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetRole(int id)
        {
            var result = await _applicationUserService.GetRoleAsync(id);
            if (result != null)
                return Ok(result);
            return NotFound();
        }

        [HttpGet("roles"), Authorize(Roles = "Admin, Seller, Manager")]
        public async Task<IActionResult> GetRoles()
        {
            var result = await _applicationUserService.GetAllRolesAsync();
            if (result != null)
                return Ok(result);
            return NotFound();
        }

        [HttpPost("{id}/roles"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddRoles(UpdateRolesDto request, int id)
        {
            var result = await _applicationUserService.AddUserRolesAsync(request, id);
            if (result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);
        }

        [HttpPut("sent-messages/{id}"), Authorize]
        public async Task<IActionResult> UpdateSentMessage(UpdateMessageDto request, int id)
        {
            var result = await _applicationUserService.UpdateSentMessageAsync(request, id);
            if (result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);
        }

        [HttpPut("me/services/{id}/status"), Authorize(Roles = "Serviceman")]
        public async Task<IActionResult> UpdateServiceStatusServiceman(UpdateServiceDto request, int id)
        {
            var result = await _serviceService.UpdateServiceStatus(request, id);
            if (result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);
        }

        [HttpDelete("sent-messages/{id}"), Authorize]
        public async Task<IActionResult> DeleteSentMessage(int id)
        {
            var result = await _applicationUserService.DeleteSentMessageAsync(id);
            if (result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);
        }

        [HttpDelete("{id}/roles"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteRoles(UpdateRolesDto request, int id)
        {
            var result = await _applicationUserService.RemoveUserRolesAsync(request, id);
            if (result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);
        }

        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _applicationUserService.DeleteUserAsync(id);
            if (result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);
        }
    }
}
