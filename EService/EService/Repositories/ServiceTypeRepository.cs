using EService.Data;
using EService.Models;
using EService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System.Transactions;

namespace EService.Repositories
{
    public class ServiceTypeRepository : IServiceTypeRepository
    {
        private readonly ApplicationDbContext _context;

        public ServiceTypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ServiceType?> GetServiceTypeByNameAsync(string name)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                                   new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                                   TransactionScopeAsyncFlowOption.Enabled);
            ServiceType? servType = null;
            try
            {
                servType = await Task.Run(() => _context.ServiceTypes.Where(s => s.Name == name).
                    Include(s => s.Services).
                    FirstOrDefaultAsync());
                scope.Complete();
            }
            catch (Exception) { }
            return await Task.Run(() => servType);
        }
        public async Task<ServiceType?> GetServiceTypeByIdAsync(int id)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                                   new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                                   TransactionScopeAsyncFlowOption.Enabled);
            ServiceType? servType = null;
            try
            {
                servType = await Task.Run(() => _context.ServiceTypes.Where(st => st.Id == id).
                    Include(s => s.Services).
                    FirstOrDefaultAsync());
                scope.Complete();
            }
            catch (Exception) { }
            return await Task.Run(() => servType);
        }
        public async Task<List<ServiceType>> GetAllServiceTypesAsync()
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                                   new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                                   TransactionScopeAsyncFlowOption.Enabled);
            List<ServiceType> arr = new List<ServiceType>();
            try
            {
                arr = await Task.Run(() => _context.ServiceTypes.
                    Include(s => s.Services).
                    ToListAsync());
                scope.Complete();
            }
            catch (Exception) { }
            return await Task.Run(() => arr);
        }
        public async Task AddServiceTypeAsync(ServiceType serviceType)
        {
            await _context.AddAsync(serviceType);
            await SaveChangesAsync();
        }
        public async Task RemoveServiceTypeAsync(ServiceType serviceType)
        {
            _context.Remove(serviceType);
            await SaveChangesAsync();
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
