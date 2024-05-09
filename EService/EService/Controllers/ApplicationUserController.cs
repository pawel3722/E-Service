using EService.Dtos.OrderDtos;
using EService.Dtos.RolesDtos;
using EService.Services;
using EService.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUserController : ControllerBase
    {
        private readonly IApplicationUserService _applicationUserService;

        public ApplicationUserController(IApplicationUserService applicationUserService)
        {
            _applicationUserService = applicationUserService;
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _applicationUserService.GetUserAsync(id);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _applicationUserService.GetAllUsersAsync();
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }

        [HttpPost("{id}/roles")]
        public async Task<IActionResult> AddRoles(UpdateRolesDto request, int id)
        {
            var result = await _applicationUserService.AddUserRolesAsync(request, id);
            if (result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);
        }

        [HttpDelete("{id}/roles")]
        public async Task<IActionResult> DeleteRoles(UpdateRolesDto request, int id)
        {
            var result = await _applicationUserService.RemoveUserRolesAsync(request, id);
            if (result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _applicationUserService.DeleteUserAsync(id);
            if (result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);
        }
    }
}
