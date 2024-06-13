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
            return await Task.Run(() => _context.Roles.Where(r => r.Id == id).
            Include(u => u.Users).
            FirstOrDefaultAsync());
        }
        public async Task<Role?> GetRoleByNameAsync(string name)
        {
            return await Task.Run(() => _context.Roles.Where(r => r.Name == name).
            Include(u => u.Users).
            FirstOrDefaultAsync());
        }
        public async Task<List<Role>> GetAllRolesAsync()
        {
            return await Task.Run(() => _context.Roles.
            Include(u => u.Users). //?
            ToListAsync());
        }
        public async Task<ApplicationUser?> GetUserByIdAsync(int id)
        {
            return await Task.Run(() => _context.Users.Where(u => u.Id == id).
            Include(u => u.Roles).
            Include(u => u.SentMessages).
            Include(u => u.ReceivedMessages).
            Include(u => u.ManagerOrders).
            Include(u => u.CustomerOrders).
            Include(u => u.Services).
            FirstOrDefaultAsync());
        }
        public async Task<List<ApplicationUser>> GetAllUsersAsync()
        {
            return await Task.Run(() => _context.Users.
            Include(u => u.Roles).
            Include(u => u.SentMessages).
            Include(u => u.ReceivedMessages).
            Include(u => u.ManagerOrders).
            Include(u => u.CustomerOrders).
            Include(u => u.Services).
            ToListAsync());
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
