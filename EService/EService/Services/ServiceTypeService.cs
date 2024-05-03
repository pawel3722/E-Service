using Azure.Core;
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
        public async Task<List<ServiceType>> GetAllServiceTypes()
        {
            return await _serviceTypeRepository.GetAllServiceTypes();
        }
        public async Task<ServiceType?> GetServiceType(int id)
        {
            return await _serviceTypeRepository.GetServiceTypeById(id);
        }
        public async Task<(bool Confirmed, string Response)> CreateServiceType(ServiceTypeDto request)
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
        public async Task<(bool Confirmed, string Response)> UpdateServiceType(ServiceTypeDto request, int id)
        {
            var serviceType = await _serviceTypeRepository.GetServiceTypeById(id);
            if (serviceType != null)
            {
                if (request.MaxPrice >= request.MinPrice)
                {
                    serviceType.Name = request.Name;
                    serviceType.MinPrice = request.MinPrice;
                    serviceType.MaxPrice = request.MaxPrice;
                    await _serviceTypeRepository.SaveChangesAsync();
                    return await Task.FromResult((true, "Service type successfully updated."));
                }
                else return await Task.FromResult((false, "Max value must be greater than min value."));
            }
            else return await Task.FromResult((false, "Service type with given id does not exist."));
        }

        public async Task<(bool Confirmed, string Response)> DeleteServiceType(int id)
        {
            var serviceType = await _serviceTypeRepository.GetServiceTypeById(id);
            if (serviceType != null)
            {
                await _serviceTypeRepository.RemoveServiceTypeAsync(serviceType);
                return await Task.FromResult((true, "Service type successfully deleted."));
            }
            else return await Task.FromResult((false, "Service type with given id does not exist."));
        }
    }
}
