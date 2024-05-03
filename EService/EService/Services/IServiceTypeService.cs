using Azure.Core;
using EService.Dtos.ServiceTypeDtos;
using EService.Models;
using System.Threading.Tasks;

namespace EService.Services
{
    public interface IServiceTypeService
    {
        public Task<ServiceType?> GetServiceType(int id);
        public Task<List<ServiceType>> GetAllServiceTypes();
        public Task<(bool Confirmed, string Response)> CreateServiceType(ServiceTypeDto request);
        public Task<(bool Confirmed, string Response)> UpdateServiceType(ServiceTypeDto request, int id);
        public Task<(bool Confirmed, string Response)> DeleteServiceType(int id);
    }
}
