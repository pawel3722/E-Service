using EService.Dtos.OrderDtos;
using EService.Dtos.ReviewDtos;
using EService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService service)
        {
            _orderService = service;
        }


        [HttpGet("{id}"), Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _orderService.GetOrderAsync(id);
            if (result != null)
                return Ok(result);
            return NotFound();
        }

        [HttpGet, Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Get()
        {
            var result = await _orderService.GetAllOrdersAsync();
            if (result != null)
                return Ok(result);
            return NotFound();
        }

        [HttpGet("{id}/review"), Authorize(Roles = "Client")]
        public async Task<IActionResult> GetReview(int id)
        {
            var result = await _orderService.GetReviewFromOrderAsync(id);
            if (result.Confirmed) 
                return Ok(result.Review);
            else return BadRequest(result.Response);
        }

        [HttpPost, Authorize(Roles = "Admin,Seller")]
        public async Task<IActionResult> Create(CreateOrderDto request)
        {
            var result = await _orderService.CreateOrderAsync(request);
            if (result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);
        }

        [HttpPut("{id}"), Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Update(UpdateOrderDto request, int id)
        {
            var result = await _orderService.UpdateOrderAsync(request, id);
            if (result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);
        }

        [HttpPut("{id}/status"), Authorize(Roles = "Manager,Seller")]
        public async Task<IActionResult> UpdateStatus(UpdateOrderDto request, int id)
        {
            var result = await _orderService.UpdateOrderStatusAsync(request, id);
            if (result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);
        }

        [HttpPut("{id}/paid"), Authorize(Roles = "Seller")]
        public async Task<IActionResult> UpdatePaidSeller(int id)
        {
            var result = await _orderService.UpdateOrderPaidSellerAsync(id);
            if (result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);
        }

        [HttpPut("{id}/services"), Authorize(Roles = "Serviceman")]
        public async Task<IActionResult> UpdateOrderServices(UpdateOrderDto request, int id)
        {
            var result = await _orderService.UpdateOrderServicesAsync(request, id);
            if (result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);
        }

        [HttpPut("{id}/review"), Authorize(Roles = "Client")]
        public async Task<IActionResult> UpdateSentReview(UpdateReviewDto request, int id)
        {
            var result = await _orderService.UpdateSentReview(request, id);
            if (result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);
        }

        [HttpDelete("{id}"), Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _orderService.DeleteOrderAsync(id);
            if (result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);
        }

        [HttpDelete("{id}/review"), Authorize(Roles = "Client")]
        public async Task<IActionResult> DeleteSentReview(int id)
        {
            var result = await _orderService.DeleteSentReview(id);
            if (result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);
        }
    }
}
