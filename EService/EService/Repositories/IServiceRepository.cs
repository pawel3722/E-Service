using EService.Models;

namespace EService.Repositories
{
    public interface IServiceRepository
    {
        public Task<Service?> GetServiceById(int id);
        public Task<List<Service>> GetAllServices();
        public Task AddServiceAsync(Service Service);
        public Task RemoveServiceAsync(Service Service);
        public Task SaveChangesAsync();
    }
}
