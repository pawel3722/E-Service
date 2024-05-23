using EService.Dtos.ReviewDtos;
using EService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService service)
        {
            _reviewService = service;
        }

        [HttpGet("{id}"), Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _reviewService.GetReviewAsync(id);
            if (result != null)
                return Ok(result);
            return NotFound();
        }

        [HttpGet, Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Get()
        {
            var result = await _reviewService.GetAllReviewsAsync();
            if (result != null)
                return Ok(result);
            return NotFound();
        }

        [HttpPost, Authorize(Roles = "Client")]
        public async Task<IActionResult> Create(CreateReviewDto request)
        {
            var result = await _reviewService.CreateReviewAsync(request);
            if (result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);
        }

        [HttpPut("admin/{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(UpdateReviewDto request, int id)
        {
            var result = await _reviewService.UpdateReview(request, id);
            if (result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);
        }

        [HttpPut("{id}"), Authorize(Roles = "Client")]
        public async Task<IActionResult> UpdateSentReview(UpdateReviewDto request, int id)
        {
            var result = await _reviewService.UpdateSentReview(request, id);
            if (result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);
        }

        [HttpDelete("admin/{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _reviewService.DeleteReview(id);
            if (result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);
        }

        [HttpDelete("{id}"), Authorize(Roles = "Client")]
        public async Task<IActionResult> DeleteSentReview(int id)
        {
            var result = await _reviewService.DeleteSentReview(id);
            if (result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);
        }
    }
}
