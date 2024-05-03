using EService.Dtos.ModelDtos;
using EService.Dtos.ServiceTypeDtos;
using EService.Services;
using Microsoft.AspNetCore.Http;
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

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _modelService.GetModel(id);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _modelService.GetAllModels();
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ModelDto request)
        {
            var result = await _modelService.CreateModel(request);
            if (result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(ModelDto request, int id)
        {
            var result = await _modelService.UpdateModel(request, id);
            if (result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _modelService.DeleteModel(id);
            if (result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);
        }
    }
}
