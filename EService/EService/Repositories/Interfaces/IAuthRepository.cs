using EService.Models;

namespace EService.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        public Task<Role?> GetRoleByNameAsync(string name); 
        public Task<ApplicationUser?> GetUserByIdAsync(int id); 
        public Task<ApplicationUser?> GetUserByEmailAsync(string email); 
        public Task<ApplicationUser?> GetUserByRefreshTokenAsync(string refreshToken); 
        public Task<bool> UserExistsAsync(string email); 
        public Task AddUserAsync(ApplicationUser user); 
        public Task AddRoleAsync(Role role); 
        public Task SaveChangesAsync(); 
    }
}
