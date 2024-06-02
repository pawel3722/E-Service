using EService.Dtos.ServiceDtos;
using EService.Services;
using EService.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly IServiceService _serviceService;

        public ServiceController(IServiceService service)
        {
            _serviceService = service;
        }

        [HttpGet("{id}"), Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _serviceService.GetService(id);
            if (result != null)
                return Ok(result);
            return NotFound();
        }

        [HttpGet, Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Get()
        {
            var result = await _serviceService.GetAllServices();
            if (result != null)
                return Ok(result);
            return NotFound();
        }

        [HttpPost, Authorize(Roles = "Admin,Seller,Manager,Serviceman")]
        public async Task<IActionResult> Create(CreateServiceDto request)
        {
            var result = await _serviceService.CreateService(request);
            if (result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);
        }

        [HttpPut("{id}"), Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> Update(UpdateServiceDto request, int id)
        {
            var result = await _serviceService.UpdateService(request, id);
            if (result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);
        }

        [HttpPut("{id}/status"), Authorize(Roles = "Serviceman")]
        public async Task<IActionResult> UpdateServiceStatusServiceman(UpdateServiceDto request, int id)
        {
            var result = await _serviceService.UpdateServiceStatus(request, id);
            if (result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);
        }

        [HttpDelete("{id}"), Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _serviceService.DeleteService(id);
            if (result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);
        }
    }
}
