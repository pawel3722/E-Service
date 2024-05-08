using EService.Dtos.ServiceDtos;
using EService.Models;
using EService.Repositories;

namespace EService.Services
{
    public class ServiceService : IServiceService
    {
        private readonly IServiceRepository _serviceRepository;
        public ServiceService(IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }
        public async Task<Service?> GetService(int id)
        {
            return await _serviceRepository.GetServiceById(id);

        }
        public async Task<List<Service>> GetAllServices()
        {
            return await _serviceRepository.GetAllServices();

        }
        public async Task<(bool Confirmed, string Response)> CreateService(ServiceDto request)
        {
            var service = new Service
            {
                PartPrice = request.PartPrice,
                ServicePrice = request.ServicePrice,
                OrderId = request.OrderId,
                ServiceTypeId = request.ServiceTypeId,
                PartId = request.PartId
            };
            await _serviceRepository.AddServiceAsync(service);
            return await Task.FromResult((true, "Service successfully created."));
        }
        public async Task<(bool Confirmed, string Response)> UpdateService(ServiceDto request, int id)
        {
            var service = await _serviceRepository.GetServiceById(id);
            if (service != null)
            {
                service.PartPrice = request.PartPrice;
                service.ServicePrice = request.ServicePrice;
                service.OrderId = request.OrderId;
                service.ServiceTypeId = request.ServiceTypeId;
                service.PartId = request.PartId;
                await _serviceRepository.SaveChangesAsync();
                return await Task.FromResult((true, "Service successfully updated."));
            }
            else return await Task.FromResult((false, "Service with given id does not exist."));
        }
        public async Task <(bool Confirmed, string Response)> DeleteService(int id)
        {
            var service = await _serviceRepository.GetServiceById(id);
            if (service != null)
            {
                await _serviceRepository.RemoveServiceAsync(service);
                return await Task.FromResult((true, "Service successfully deleted."));
            }
            else return await Task.FromResult((false, "Service with given id does not exist."));
        }
    }
}
