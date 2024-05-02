using E2_Service.Dtos;

namespace E2_Service.Services
{
    public interface IAuthService
    {
        public Task<(bool Confirmed, string Response)> RegisterUser(UserRegisterRequestDto request);
        public Task<(bool Confirmed, string Response, TokensResponseDto? Tokens)> LoginUser(UserLoginRequestDto request);
        public Task<(bool Confirmed, string Response, TokensResponseDto? Tokens)> RefreshToken();
    }
}
