using EService.Data;
using EService.Models;
using EService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EService.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDbContext _context;
        public AuthRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Role?> GetRoleByNameAsync(string name)
        {
            return await Task.Run(() => _context.Roles.Where(r => r.Name == name).
            FirstOrDefaultAsync());
        }
        public async Task<ApplicationUser?> GetUserByIdAsync(int id)
        {
            return await Task.Run(() => _context.Users.Where(u => u.Id == id).
            Include(u => u.Roles).
            FirstOrDefaultAsync());
        }
        public async Task<ApplicationUser?> GetUserByEmailAsync(string email)
        {
            return await Task.Run(() => _context.Users.Where(u => u.Email == email).
            Include(u => u.Roles).
            FirstOrDefaultAsync());
        }
        public async Task<ApplicationUser?> GetUserByRefreshTokenAsync(string refreshToken)
        {
            return await Task.Run(() => _context.Users.Where(u => u.RefreshToken == refreshToken).
            Include(u => u.Roles).
            FirstOrDefaultAsync());
        }
        public async Task<bool> UserExistsAsync(string email)
        {
            return await Task.Run(() => _context.Users.Where(u => u.Email == email).AnyAsync());
        }
        public async Task AddUserAsync(ApplicationUser user)
        {
            await _context.Users.AddAsync(user);
            await SaveChangesAsync();
        }
        public async Task AddRoleAsync(Role role)
        {
            await _context.Roles.AddAsync(role);
            await SaveChangesAsync();
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
