using EService.Data;
using EService.Models;
using Microsoft.EntityFrameworkCore;

namespace EService.Repositories
{
    public class ServiceTypeRepository : IServiceTypeRepository
    {
        private readonly ApplicationDbContext _context;

        public ServiceTypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ServiceType?> GetServiceTypeByName(string name)
        {
            return await Task.Run(() => _context.ServiceTypes.Where(s => s.Name == name).FirstOrDefaultAsync());
        }
        public async Task AddServiceTypeAsync(ServiceType serviceType)
        {
            await _context.AddAsync(serviceType);
            await SaveChangesAsync();
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
