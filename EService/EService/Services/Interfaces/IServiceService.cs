using EService.Dtos.ServiceDtos;
using EService.Models;
using Microsoft.AspNetCore.Mvc;

namespace EService.Services
{
    public interface IServiceService
    {
        public Task<Service?> GetService(int id);
        public Task<List<Service>> GetAllServices();
        public Task<(bool Confirmed, string Response)> CreateService(CreateServiceDto request);
        public Task<(bool Confirmed, string Response)> UpdateService(UpdateServiceDto request, int id);
        public Task<(bool Confirmed, string Response)> UpdateServiceStatus(UpdateServiceDto request, int id);
        public Task<(bool Confirmed, string Response)> DeleteService(int id);
    }
}
