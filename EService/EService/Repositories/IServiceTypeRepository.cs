using EService.Models;

namespace EService.Repositories
{
    public interface IServiceTypeRepository
    {
        public Task<ServiceType?> GetServiceTypeByName(string name);
        public Task<ServiceType?> GetServiceTypeById(int id);
        public Task<List<ServiceType>> GetAllServiceTypes();
        public Task AddServiceTypeAsync(ServiceType serviceType);
        public Task RemoveServiceTypeAsync(ServiceType serviceType);
        public Task SaveChangesAsync();
    }
}
