using EService.Dtos.PartDtos;
using EService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace EService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("_myAllowSpecificOrigins")]
    public class PartController : ControllerBase
    {
        private readonly IPartService _partService;

        public PartController(IPartService service)
        {
            _partService = service;
        }

        [HttpGet("{id}"), Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _partService.GetPartAsync(id);
            if (result != null)
                return Ok(result);
            return NotFound();
        }

        [HttpGet, Authorize(Roles = "Admin,Manager,Serviceman")]
        public async Task<IActionResult> Get()
        {
            var result = await _partService.GetAllPartsAsync();
            if (result != null)
                return Ok(result);
            return NotFound();
        }

        [HttpPost, Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Create(CreatePartDto request)
        {
            var result = await _partService.CreatePartAsync(request);
            if (result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);
        }

        [HttpPut("{id}"), Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Update(UpdatePartDto request, int id)
        {
            var result = await _partService.UpdatePartAsync(request, id);
            if (result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);
        }

        [HttpDelete("{id}"), Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _partService.DeletePartAsync(id);
            if (result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);
        }

    }
}
