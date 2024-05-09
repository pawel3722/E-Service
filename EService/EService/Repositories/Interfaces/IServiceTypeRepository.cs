using EService.Models;

namespace EService.Repositories.Interfaces
{
    public interface IServiceTypeRepository
    {
        public Task<ServiceType?> GetServiceTypeByNameAsync(string name);
        public Task<ServiceType?> GetServiceTypeByIdAsync(int id);
        public Task<List<ServiceType>> GetAllServiceTypesAsync();
        public Task AddServiceTypeAsync(ServiceType serviceType);
        public Task RemoveServiceTypeAsync(ServiceType serviceType);
        public Task SaveChangesAsync();
    }
}
