using EService.Dtos.ServiceDtos;
using EService.Models;

namespace EService.Services
{
    public interface IServiceService
    {
        public Task<Service?> GetService(int id);
        public Task<List<Service>> GetAllServices();
        public Task<(bool Confirmed, string Response)> CreateService(ServiceDto request);
        public Task<(bool Confirmed, string Response)> UpdateService(ServiceDto request, int id);
        public Task<(bool Confirmed, string Response)> DeleteService(int id);
    }
}
