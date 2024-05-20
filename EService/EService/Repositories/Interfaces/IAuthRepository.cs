using EService.Models;

namespace EService.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        public Task<Role?> GetRoleByIdAsync(int id);
        public Task<Role?> GetRoleByNameAsync(string name);
        public Task<List<ApplicationUser>> GetAllUsersAsync();
        public Task<ApplicationUser?> GetUserByIdAsync(int id);
        public Task<ApplicationUser?> GetUserByEmailAsync(string email);
        public Task<ApplicationUser?> GetUserByRefreshTokenAsync(string refreshToken);
        public Task<List<Role>> GetAllRolesAsync();
        public Task<Role?> GetRoleAsync(string name);
        public Task<bool> UserExistsAsync(string email);
        public Task AddUserAsync(ApplicationUser user);
        public Task AddRoleAsync(Role role);
        public Task RemoveUserAsync(ApplicationUser user);
        public Task SaveChangesAsync();
    }
}
