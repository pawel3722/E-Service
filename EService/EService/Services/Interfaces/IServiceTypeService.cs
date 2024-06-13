using Azure.Core;
using EService.Dtos.ServiceTypeDtos;
using EService.Models;
using System.Threading.Tasks;

namespace EService.Services
{
    public interface IServiceTypeService
    {
        public Task<ReturnServiceTypeDto?> GetServiceTypeAsync(int id);
        public Task<List<ReturnServiceTypeDto>> GetAllServiceTypesAsync();
        public Task<(bool Confirmed, string Response)> CreateServiceTypeAsync(CreateServiceTypeDto request);
        public Task<(bool Confirmed, string Response)> UpdateServiceTypeAsync(UpdateServiceTypeDto request, int id);
        public Task<(bool Confirmed, string Response)> DeleteServiceTypeAsync(int id);
    }
}
