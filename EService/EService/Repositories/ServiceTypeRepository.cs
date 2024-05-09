using EService.Data;
using EService.Models;
using EService.Repositories.Interfaces;
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
        public async Task<ServiceType?> GetServiceTypeByNameAsync(string name)
        {
            return await Task.Run(() => _context.ServiceTypes.Where(s => s.Name == name).FirstOrDefaultAsync());
        }
        public async Task<ServiceType?> GetServiceTypeByIdAsync(int id)
        {
            return await Task.Run(() => _context.ServiceTypes.FirstOrDefaultAsync(st => st.Id == id));
        }
        public async Task<List<ServiceType>> GetAllServiceTypesAsync()
        {
            return await Task.Run(() => _context.ServiceTypes.ToListAsync());
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
