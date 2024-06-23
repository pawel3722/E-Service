using EService.Data;
using EService.Models;
using EService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;
using System.Transactions;

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
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                                    new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                                    TransactionScopeAsyncFlowOption.Enabled);
            Role? role = null;
            try
            {
                role = await Task.Run(() => _context.Roles.Where(r => r.Name == name).FirstOrDefaultAsync());
                scope.Complete();
            }
            catch (Exception) { }
            return await Task.Run(() => role);
        }
        public async Task<ApplicationUser?> GetUserByIdAsync(int id)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                                    new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                                    TransactionScopeAsyncFlowOption.Enabled);
            ApplicationUser? usr = null;
            try
            {
                usr = await Task.Run(() => _context.Users.Where(u => u.Id == id).
                                                           Include(u => u.Roles).
                                                           FirstOrDefaultAsync());
                scope.Complete();
            }
            catch (Exception) { }
            return await Task.Run(() => usr);
        }
        public async Task<ApplicationUser?> GetUserByEmailAsync(string email)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                                    new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                                    TransactionScopeAsyncFlowOption.Enabled);
            ApplicationUser? usr = null;
            try
            {
                usr = await Task.Run(() => _context.Users.Where(u => u.Email == email).
                                                          Include(u => u.Roles).
                                                          FirstOrDefaultAsync());
                scope.Complete();
            }
            catch (Exception) { }
            return await Task.Run(() => usr);
        }
        public async Task<ApplicationUser?> GetUserByRefreshTokenAsync(string refreshToken)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                                    new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                                    TransactionScopeAsyncFlowOption.Enabled);
            ApplicationUser? usr = null;
            try
            {
                usr = await Task.Run(() => _context.Users.Where(u => u.RefreshToken == refreshToken).
                                                          Include(u => u.Roles).
                                                          FirstOrDefaultAsync());
                scope.Complete();
            }
            catch (Exception) { }
            return await Task.Run(() => usr);
        }
        public async Task<bool> UserExistsAsync(string email)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                                    new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                                    TransactionScopeAsyncFlowOption.Enabled);
            bool exists = false;
            try
            {
                exists = await Task.Run(() => _context.Users.Where(u => u.Email == email).AnyAsync());
                scope.Complete();
            }
            catch (Exception) { }
            return await Task.Run(() => exists);
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
