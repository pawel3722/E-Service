using EService.Data;
using EService.Models;
using EService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EService.Repositories
{
    public class ApplicationUserRepository : IApplicationUserRepository
    {
        private readonly ApplicationDbContext _context;
        public ApplicationUserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Role?> GetRoleByIdAsync(int id)
        {
            return await Task.Run(() => _context.Roles.Where(r => r.Id == id).FirstOrDefaultAsync());
        }
        public async Task<Role?> GetRoleByNameAsync(string name)
        {
            return await Task.Run(() => _context.Roles.Where(r => r.Name == name).FirstOrDefaultAsync());
        }
        public async Task<ApplicationUser?> GetUserByIdAsync(int id)
        {
            return await Task.Run(() => _context.Users.Where(u => u.Id == id).
            Include(u => u.Roles).
            FirstOrDefaultAsync());
        }
        public async Task<List<ApplicationUser>> GetAllUsersAsync()
        {
            return await Task.Run(() => _context.Users.
            Include(m => m.Roles).
            ToListAsync());
        }
        public async Task AddUserAsync(ApplicationUser user)
        {
            await _context.AddAsync(user);
            await SaveChangesAsync();
        }
        public async Task RemoveUserAsync(ApplicationUser user)
        {
            _context.Remove(user);
            await SaveChangesAsync();
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
