using E2_Service.Dtos;
using E2_Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace E2_Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
            var result = await _authService.RegisterUser(request);
            if (result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginRequestDto request)
        {
            var result = await _authService.LoginUser(request);
            if (result.Confirmed)
                if(result.Tokens != null)
                    return Ok(result.Tokens);
                else return StatusCode(500, result.Response);
            else return BadRequest(result.Response);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh()
        {
            var result = await _authService.RefreshToken();
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
