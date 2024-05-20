using Azure.Core;
using EService.Dtos.ServiceTypeDtos;
using EService.Models;
using EService.Repositories.Interfaces;

namespace EService.Services
{
    public class ServiceTypeService : IServiceTypeService
    {
        private readonly IServiceTypeRepository _serviceTypeRepository;
        public ServiceTypeService(IServiceTypeRepository serviceTypeRepository) 
        { 
            _serviceTypeRepository = serviceTypeRepository;
        }
        public async Task<List<ServiceType>> GetAllServiceTypesAsync()
        {
            return await _serviceTypeRepository.GetAllServiceTypesAsync();
        }
        public async Task<ServiceType?> GetServiceTypeAsync(int id)
        {
            return await _serviceTypeRepository.GetServiceTypeByIdAsync(id);
        }
        public async Task<(bool Confirmed, string Response)> CreateServiceTypeAsync(CreateServiceTypeDto request)
        {
            var serviceType = await _serviceTypeRepository.GetServiceTypeByNameAsync(request.Name);
            if (serviceType == null)
            {
                if(request.MaxPrice >= request.MinPrice)
                {
                    serviceType = new ServiceType
                    {
                        Name = request.Name,
                        MinPrice = request.MinPrice,
                        MaxPrice = request.MaxPrice,
                        DeviceType = request.DeviceType
                    };
                    await _serviceTypeRepository.AddServiceTypeAsync(serviceType);
                    return await Task.FromResult((true, "Service type successfully created."));
                }
                else return await Task.FromResult((false, "Max value must be greater than min value."));
            }
            else return await Task.FromResult((false, "Such service type already exists."));
        }
        public async Task<(bool Confirmed, string Response)> UpdateServiceTypeAsync(UpdateServiceTypeDto request, int id)
        {
            var serviceType = await _serviceTypeRepository.GetServiceTypeByIdAsync(id);
            if (serviceType != null)
            {
                if (request.MaxPrice >= request.MinPrice)
                {
                    if(request.Name != null) serviceType.Name = request.Name;
                    if (request.MinPrice != null) serviceType.MinPrice = request.MinPrice.Value;
                    if (request.MaxPrice != null) serviceType.MaxPrice = request.MaxPrice.Value;
                    if (request.DeviceType != null) serviceType.DeviceType = request.DeviceType;
                    await _serviceTypeRepository.SaveChangesAsync();
                    return await Task.FromResult((true, "Service type successfully updated."));
                }
                else return await Task.FromResult((false, "Max value must be greater than min value."));
            }
            else return await Task.FromResult((false, "Service type with given id does not exist."));
        }

        public async Task<(bool Confirmed, string Response)> DeleteServiceTypeAsync(int id)
        {
            var serviceType = await _serviceTypeRepository.GetServiceTypeByIdAsync(id);
            if (serviceType != null)
            {
                await _serviceTypeRepository.RemoveServiceTypeAsync(serviceType);
                return await Task.FromResult((true, "Service type successfully deleted."));
            }
            else return await Task.FromResult((false, "Service type with given id does not exist."));
        }
    }
}
