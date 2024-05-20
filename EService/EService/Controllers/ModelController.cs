using EService.Dtos.ModelDtos;
using EService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EService.Controllers
{
    [Route("api/[controller]")]
    
    [ApiController]
    public class ModelController : ControllerBase
    {
        private readonly IModelService _modelService;

        public ModelController(IModelService service)
        {
            _modelService = service;
        }

        [HttpGet("{id}"), Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _modelService.GetModelAsync(id);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }

        [HttpGet, Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Get()
        {
            var result = await _modelService.GetAllModelsAsync();
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }

        [HttpPost, Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Create(CreateModelDto request)
        {
            var result = await _modelService.CreateModelAsync(request);
            if (result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);
        }

        [HttpPut("{id}"), Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Update(UpdateModelDto request, int id)
        {
            var result = await _modelService.UpdateModelAsync(request, id);
            if (result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);
        }

        [HttpDelete("{id}"), Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _modelService.DeleteModelAsync(id);
            if (result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);
        }
    }
}
