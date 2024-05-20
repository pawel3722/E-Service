using EService.Dtos.AuthDtos;
using EService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("example"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> Example()
        {
            return Ok("It worked.");
        }
        [HttpGet("example2"), Authorize(Roles = "Client")]
        public async Task<IActionResult> Example2()
        {
            return Ok("It worked too.");
        }
    }
}
