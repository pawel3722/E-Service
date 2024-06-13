using EService.Dtos.ApplicationUserDtos;
using EService.Dtos.AuthDtos;
using EService.Dtos.MessageDtos;
using EService.Dtos.RolesDtos;
using EService.Models;

namespace EService.Services
{
    public interface IAuthService
    {
        public Task<ReturnApplicationUserDto?> GetUserAsync(int id);
        public Task<(bool Confirmed, string Response)> RegisterUserAsync(UserRegisterRequestDto request);
        public Task<(bool Confirmed, string Response, TokensResponseDto? Tokens)> LoginUserAsync(UserLoginRequestDto request);
        public Task<(bool Confirmed, string Response, TokensResponseDto? Tokens)> RefreshTokenAsync();
    }
}
