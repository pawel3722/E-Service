using EService.Dtos.AuthDtos;
using EService.Dtos.RolesDtos;
using EService.Models;

namespace EService.Services
{
    public interface IAuthService
    {
        public Task<List<ApplicationUser>> GetAllUsersAsync();
        public Task<List<Role>> GetAllRolesAsync();
        public Task<ApplicationUser?> GetUserAsync(int id);
        public Task<Role?> GetRoleAsync(int id);
        public Task<(bool Confirmed, string Response)> AddUserRolesAsync(UpdateRolesDto request, int id);
        public Task<(bool Confirmed, string Response)> RemoveUserRolesAsync(UpdateRolesDto request, int id);
        public Task<(bool Confirmed, string Response)> DeleteUserAsync(int id);
        public Task<(bool Confirmed, string Response)> RegisterUserAsync(UserRegisterRequestDto request);
        public Task<(bool Confirmed, string Response, TokensResponseDto? Tokens)> LoginUserAsync(UserLoginRequestDto request);
        public Task<(bool Confirmed, string Response, TokensResponseDto? Tokens)> RefreshTokenAsync();
    }
}
