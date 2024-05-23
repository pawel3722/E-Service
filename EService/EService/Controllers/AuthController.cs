using EService.Dtos.AuthDtos;
using EService.Dtos.RolesDtos;
using EService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("_myAllowSpecificOrigins")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterRequestDto request)
        {
            var result = await _authService.RegisterUserAsync(request);
            if (result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginRequestDto request)
        {
            var result = await _authService.LoginUserAsync(request);
            if (result.Confirmed)
                if (result.Tokens != null)
                    return Ok(result.Tokens);
                else return StatusCode(500, result.Response);
            else return BadRequest(result.Response);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh()
        {
            var result = await _authService.RefreshTokenAsync();
            if (result.Confirmed)
                if (result.Tokens != null)
                    return Ok(result.Tokens);
                else return StatusCode(500, result.Response);
            else return BadRequest(result.Response);
        }

        [HttpGet("admin/users/{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUser(int id)
        {
            var result = await _authService.GetUserAsync(id);
            if (result != null)
                return Ok(result);
            return NotFound();
        }
        [HttpGet("users/me"), Authorize]
        public async Task<IActionResult> GetMe()
        {
            var result = await _authService.GetUserAsync(Int32.Parse(Request.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)!));
            if (result != null)
                return Ok(result);
            return NotFound();
        }

        [HttpGet("admin/users"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUsers()
        {
            var result = await _authService.GetAllUsersAsync();
            if (result != null)
                return Ok(result);
            return NotFound();
        }
        [HttpGet("admin/roles/{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetRole(int id)
        {
            var result = await _authService.GetRoleAsync(id);
            if (result != null)
                return Ok(result);
            return NotFound();
        }

        [HttpGet("admin/roles"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetRoles()
        {
            var result = await _authService.GetAllRolesAsync();
            if (result != null)
                return Ok(result);
            return NotFound();
        }

        [HttpPost("admin/users/{id}/roles"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddRoles(UpdateRolesDto request, int id)
        {
            var result = await _authService.AddUserRolesAsync(request, id);
            if (result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);
        }

        [HttpDelete("admin/users/{id}/roles"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteRoles(UpdateRolesDto request, int id)
        {
            var result = await _authService.RemoveUserRolesAsync(request, id);
            if (result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);
        }

        [HttpDelete("admin/users/{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _authService.DeleteUserAsync(id);
            if (result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);
        }
    }
}
