using Azure.Core;
using EService.Dtos.ServiceTypeDtos;

namespace EService.Services
{
    public interface IServiceTypeService
    {
        public Task<(bool Confirmed, string Response)> CreateService(CreateServiceTypeDto request);
    }
}
