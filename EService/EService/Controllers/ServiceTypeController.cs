using EService.Dtos.ServiceTypeDtos;
using EService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceTypeController : ControllerBase
    {
        private readonly IServiceTypeService _serviceTypeService;

        public ServiceTypeController(IServiceTypeService service)
        {
            _serviceTypeService = service;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _serviceTypeService.GetServiceTypeAsync(id);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _serviceTypeService.GetAllServiceTypesAsync();
            if(result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }

        [HttpPost, Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Create(CreateServiceTypeDto request)
        {
            var result = await _serviceTypeService.CreateServiceTypeAsync(request);
            if (result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);
        }

        [HttpPut("{id}"), Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Update(UpdateServiceTypeDto request, int id)
        {
            var result = await _serviceTypeService.UpdateServiceTypeAsync(request, id);
            if(result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);
        }

        [HttpDelete("{id}"), Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _serviceTypeService.DeleteServiceTypeAsync(id);
            if(result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);
        }
    }
}
