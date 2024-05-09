using EService.Models;
using Microsoft.EntityFrameworkCore;

namespace EService.Repositories.Interfaces
{
    public interface IApplicationUserRepository
    {
        public Task<Role?> GetRoleByIdAsync(int id);
        public Task<Role?> GetRoleByNameAsync(string name);
        public Task<ApplicationUser?> GetUserByIdAsync(int id);
        public Task<List<ApplicationUser>> GetAllUsersAsync();
        public Task AddUserAsync(ApplicationUser user);
        public Task RemoveUserAsync(ApplicationUser user);
        public Task SaveChangesAsync();
    }
}
