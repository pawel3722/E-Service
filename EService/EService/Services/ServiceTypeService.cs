using AutoMapper;
using Azure.Core;
using EService.Dtos.ServiceDtos;
using EService.Dtos.ServiceTypeDtos;
using EService.Models;
using EService.Repositories;
using EService.Repositories.Interfaces;

namespace EService.Services
{
    public class ServiceTypeService : IServiceTypeService
    {
        private readonly IServiceTypeRepository _serviceTypeRepository;
        private readonly IMapper _mapper;
        public ServiceTypeService(IServiceTypeRepository serviceTypeRepository, IMapper mapper) 
        { 
            _serviceTypeRepository = serviceTypeRepository;
            _mapper = mapper;
        }
        public async Task<List<ReturnServiceTypeDto>> GetAllServiceTypesAsync()
        {
            var review = await _serviceTypeRepository.GetAllServiceTypesAsync();
            return _mapper.Map<List<ReturnServiceTypeDto>>(review);
            //return await _serviceTypeRepository.GetAllServiceTypesAsync();
        }
        public async Task<ReturnServiceTypeDto?> GetServiceTypeAsync(int id)
        {
            var review = await _serviceTypeRepository.GetServiceTypeByIdAsync(id);
            return _mapper.Map<ReturnServiceTypeDto>(review);
           // return await _serviceTypeRepository.GetServiceTypeByIdAsync(id);
        }
        public async Task<(bool Confirmed, string Response)> CreateServiceTypeAsync(CreateServiceTypeDto request)
        {
            var serviceType = await _serviceTypeRepository.GetServiceTypeByNameAsync(request.Name);
            if(serviceType != null) return await Task.FromResult((false, "Service type with given name already exists."));
            if(request.MaxPrice < request.MinPrice) return await Task.FromResult((false, "Max value must be greater than min value."));
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
        public async Task<(bool Confirmed, string Response)> UpdateServiceTypeAsync(UpdateServiceTypeDto request, int id)
        {
            var serviceType = await _serviceTypeRepository.GetServiceTypeByIdAsync(id);
            if (serviceType == null) return await Task.FromResult((false, "Service type with given id does not exist."));
            if (request.MaxPrice < request.MinPrice) return await Task.FromResult((false, "Max value must be greater than min value."));
            if(request.Name != null) serviceType.Name = request.Name;
            if (request.MinPrice != null) serviceType.MinPrice = request.MinPrice.Value;
            if (request.MaxPrice != null) serviceType.MaxPrice = request.MaxPrice.Value;
            if (request.DeviceType != null) serviceType.DeviceType = request.DeviceType;
            await _serviceTypeRepository.SaveChangesAsync();
            return await Task.FromResult((true, "Service type successfully updated."));
        }

        public async Task<(bool Confirmed, string Response)> DeleteServiceTypeAsync(int id)
        {
            var serviceType = await _serviceTypeRepository.GetServiceTypeByIdAsync(id);
            if (serviceType == null) return await Task.FromResult((false, "Service type with given id does not exist."));
            await _serviceTypeRepository.RemoveServiceTypeAsync(serviceType);
            return await Task.FromResult((true, "Service type successfully deleted."));
        }
    }
}
