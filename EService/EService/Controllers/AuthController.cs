using EService.Dtos.AuthDtos;
using EService.Dtos.MessageDtos;
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

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var result = await _authService.LogoutUserAsync();
            if (result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);
        }

        [HttpGet("users/me"), Authorize]
        public async Task<IActionResult> GetMe()
        {
            var result = await _authService.GetUserAsync(Int32.Parse(Request.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)!));
            if (result != null)
                return Ok(result);
            return NotFound();
        }
    }
}
