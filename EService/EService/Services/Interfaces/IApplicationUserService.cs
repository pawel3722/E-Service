using EService.Dtos.MessageDtos;
using EService.Dtos.RolesDtos;
using EService.Models;

namespace EService.Services.Interfaces
{
    public interface IApplicationUserService
    {
        public Task<List<ApplicationUser>> GetAllUsersAsync();
        public Task<ApplicationUser?> GetUserAsync(int id);
        public Task<(bool Confirmed, string Response)> AddUserRolesAsync(UpdateRolesDto request, int id);
        public Task<(bool Confirmed, string Response)> RemoveUserRolesAsync(UpdateRolesDto request, int id);
        public Task<(bool Confirmed, string Response)> DeleteUserAsync(int id);

    }
}
