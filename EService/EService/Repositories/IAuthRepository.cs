using EService.Models;

namespace EService.Repositories
{
    public interface IAuthRepository
    {
        public Task<ApplicationUser?> GetUserByEmail(string email);
        public Task<ApplicationUser?> GetUserByRefreshToken(string refreshToken);
        public Task<Role?> GetRole(string name);
        public Task<bool> UserExists(string email);
        public Task AddUser(ApplicationUser user);
        public Task SaveChangesAsync();
    }
}
