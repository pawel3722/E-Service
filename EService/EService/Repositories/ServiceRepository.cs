using EService.Data;
using EService.Models;
using EService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

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
            return await Task.Run(() => _context.Services.FirstOrDefaultAsync(s => s.Id == id));
        }
        public async Task<List<Service>> GetAllServices()
        {
            return await Task.Run(() => _context.Services.ToListAsync());
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
