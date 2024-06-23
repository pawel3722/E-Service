using EService.Data;
using EService.Models;
using EService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

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
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                                    new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                                    TransactionScopeAsyncFlowOption.Enabled);
            Role? role = null;
            try
            {
                role = await _context.Roles.Where(r => r.Id == id).FirstOrDefaultAsync();
                scope.Complete();
            }
            catch (Exception) { }
            return await Task.Run(() => role);
        }
        public async Task<Role?> GetRoleByNameAsync(string name)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                                    new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                                    TransactionScopeAsyncFlowOption.Enabled);
            Role? role = null;
            try
            {
                role = await _context.Roles.Where(r => r.Name == name).FirstOrDefaultAsync();
                scope.Complete();
            }
            catch (Exception) { }
            return await Task.Run(() => role);
        }
        public async Task<List<Role>> GetAllRolesAsync()
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                                    new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                                    TransactionScopeAsyncFlowOption.Enabled);
            List<Role> arr = new List<Role>();
            try
            {
                arr = await Task.Run(() => _context.Roles.ToListAsync());
                scope.Complete();
            }
            catch (Exception) { }
            return await Task.Run(() => arr);
        }
        public async Task<ApplicationUser?> GetUserByIdAsync(int id)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                                    new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                                    TransactionScopeAsyncFlowOption.Enabled);
            ApplicationUser? usr = null;
            try
            {
                usr = await Task.Run(() => _context.Users.Where(u => u.Id == id).Include(u => u.Roles).FirstOrDefaultAsync());
                scope.Complete();
            }
            catch (Exception) { }
            return await Task.Run(() => usr);
        }
        public async Task<List<ApplicationUser>> GetAllUsersAsync()
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                                    new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                                    TransactionScopeAsyncFlowOption.Enabled);
            List<ApplicationUser> arr = new List<ApplicationUser>();
            try
            {
                arr = await Task.Run(() => _context.Users.Include(m => m.Roles).ToListAsync());
                scope.Complete();
            }
            catch (Exception) { }
            return await Task.Run(() => arr);
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
