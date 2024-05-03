using EService.Data;
using EService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

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
        public async Task<ServiceType?> GetServiceTypeById(int id)
        {
            return await Task.Run(() => _context.ServiceTypes.FindAsync(id).Result);
        }
        public async Task<List<ServiceType>> GetAllServiceTypes()
        {
            return await Task.Run(() => _context.ServiceTypes.ToList());
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
