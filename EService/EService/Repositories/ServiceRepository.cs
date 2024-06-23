using EService.Data;
using EService.Models;
using EService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace EService.Repositories
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly ApplicationDbContext _context;

        public ServiceRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Service?> GetServiceById(int id)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                                   new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                                   TransactionScopeAsyncFlowOption.Enabled);
            Service? serv = null;
            try
            {
                serv = await Task.Run(() => _context.Services.
                    Include(s => s.Serviceman).
                    Include(s => s.Order).
                    Include(s => s.ServiceType).
                    Include(s => s.Part).
                    FirstOrDefaultAsync(s => s.Id == id));
                scope.Complete();
            }
            catch (Exception) { }
            return await Task.Run(() => serv);
        }
        public async Task<List<Service>> GetAllServices()
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                                   new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                                   TransactionScopeAsyncFlowOption.Enabled);
            List<Service> arr = new List<Service>();
            try
            {
                arr = await Task.Run(() => _context.Services.
                    Include(s => s.Serviceman).
                    Include(s => s.Order).
                    Include(s => s.ServiceType).
                    Include(s => s.Part).
                    ToListAsync());
                scope.Complete();
            }
            catch (Exception) { }
            return await Task.Run(() => arr);
        }
        public async Task AddServiceAsync(Service service)
        {
            await _context.AddAsync(service);
            await SaveChangesAsync();
        }
        public async Task RemoveServiceAsync(Service service)
        {
            _context.Remove(service);
            await SaveChangesAsync();
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
