using EService.Dtos.ServiceTypeDtos;
using EService.Models;
using EService.Repositories;

namespace EService.Services
{
    public class ServiceTypeService : IServiceTypeService
    {
        private readonly IServiceTypeRepository _serviceTypeRepository;
        public ServiceTypeService(IServiceTypeRepository serviceTypeRepository) 
        { 
            _serviceTypeRepository = serviceTypeRepository;
        }

        public async Task<(bool Confirmed, string Response)> CreateService(CreateServiceTypeDto request)
        {
            var serviceType = await _serviceTypeRepository.GetServiceTypeByName(request.Name);
            if (serviceType == null)
            {
                if(request.MaxPrice >= request.MinPrice)
                {
                    serviceType = new ServiceType
                    {
                        Name = request.Name,
                        MinPrice = request.MinPrice,
                        MaxPrice = request.MaxPrice
                    };
                    await _serviceTypeRepository.AddServiceTypeAsync(serviceType);
                    return await Task.FromResult((true, "Service type successfully created."));
                }
                else return await Task.FromResult((false, "Max value must be greater than min value."));
            }
            else return await Task.FromResult((false, "Such service type already exists."));
        }
    }
}
