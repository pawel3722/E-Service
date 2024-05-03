using EService.Models;

namespace EService.Repositories
{
    public interface IEServiceRepository
    {
        public Task<ApplicationUser?> GetUserWithEmail(string email);
        public Task<ApplicationUser?> GetUserWithRefreshToken(string refreshToken);
        public Task<Role?> GetRole(string name);
        public Task<bool> UserExists(string email);
        public Task AddUser(ApplicationUser user);
        public Task SaveChanges();
    }
}
