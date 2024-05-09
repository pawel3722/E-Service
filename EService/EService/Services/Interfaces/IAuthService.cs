using EService.Dtos.AuthDtos;

namespace EService.Services
{
    public interface IAuthService
    {
        public Task<(bool Confirmed, string Response)> RegisterUserAsync(UserRegisterRequestDto request);
        public Task<(bool Confirmed, string Response, TokensResponseDto? Tokens)> LoginUserAsync(UserLoginRequestDto request);
        public Task<(bool Confirmed, string Response, TokensResponseDto? Tokens)> RefreshTokenAsync();
    }
}
