using EService.Models;

namespace EService.Repositories
{
    public interface IServiceTypeRepository
    {
        public Task<ServiceType?> GetServiceTypeByName(string name);
        public Task AddServiceTypeAsync(ServiceType serviceType);
        public Task SaveChangesAsync();
    }
}
