using E2_Service.Data;
using E2_Service.Models;
using Microsoft.EntityFrameworkCore;

namespace E2_Service.Repositories
{
    public class E2SeviceRepository : IE2ServiceRepository
    {
        private readonly ApplicationDbContext _context;
        public E2SeviceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ApplicationUser?> GetUserWithEmail(string email)
        {
            return await Task.Run(() => _context.Users.Where(u => u.Email == email).
            Include(u => u.Roles).
            FirstOrDefaultAsync());
        }
        public async Task<ApplicationUser?> GetUserWithRefreshToken(string refreshToken)
        {
            return await Task.Run(() => _context.Users.Where(u => u.RefreshToken == refreshToken).
            Include(u => u.Roles).
            FirstOrDefaultAsync());
        }
        public async Task<Role?> GetRole(string name)
        {
            return await Task.Run(() => _context.Roles.FirstOrDefaultAsync(r => r.Name == name));
        }
        public async Task<bool> UserExists(string email)
        {
            return await Task.Run(() => _context.Users.Where(u => u.Email == email).AnyAsync());
        }
        public async Task AddUser(ApplicationUser user)
        {
            await _context.Users.AddAsync(user);
            await SaveChanges();
        }
        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}
