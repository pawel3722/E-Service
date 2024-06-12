using EService.Models;

namespace EService.Repositories.Interfaces
{
    public interface IApplicationUserRepository
    {
        public Task<Role?> GetRoleByIdAsync(int id);
        public Task<Role?> GetRoleByNameAsync(string name);
        public Task<List<Role>> GetAllRolesAsync();
        public Task<ApplicationUser?> GetUserByIdAsync(int id);
        public Task<List<ApplicationUser>> GetAllUsersAsync();
        public Task RemoveUserAsync(ApplicationUser user);
        public Task SaveChangesAsync();
    }
}
