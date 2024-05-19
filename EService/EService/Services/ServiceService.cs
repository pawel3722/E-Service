using EService.Dtos.ServiceDtos;
using EService.Models;
using EService.Repositories.Interfaces;

namespace EService.Services
{
    public class ServiceService : IServiceService
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IServiceTypeRepository _serviceTypeRepository;
        private readonly IPartRepository _partRepository;
        private readonly IAuthRepository _authRepository;
        public ServiceService(IServiceRepository serviceRepository, IOrderRepository orderRepository, IServiceTypeRepository serviceTypeRepository, IPartRepository partRepository, IAuthRepository authRepository)
        {
            _serviceRepository = serviceRepository;
            _orderRepository = orderRepository;
            _serviceTypeRepository = serviceTypeRepository;
            _partRepository = partRepository;
            _authRepository = authRepository;
        }
        public async Task<Service?> GetService(int id)
        {
            return await _serviceRepository.GetServiceById(id);
        }
        public async Task<List<Service>> GetAllServices()
        {
            return await _serviceRepository.GetAllServices();
        }
        public async Task<(bool Confirmed, string Response)> CreateService(CreateServiceDto request)
        {
            var order = await _orderRepository.GetOrderByIdAsync(request.OrderId);
            if(order != null)
            {
                var serviceType = await _serviceTypeRepository.GetServiceTypeByIdAsync(request.ServiceTypeId);
                if(serviceType != null)
                {
                    Part? part = null;
                    if (request.PartId != null)
                        part = await _partRepository.GetPartByIdAsync(request.PartId.Value);
                    var service = new Service
                    {
                        Status = request.Status,
                        PartPrice = part == null ? 0 : part.Model.Price,
                        ServicePrice = request.ServicePrice,
                        OrderId = request.OrderId,
                        Order = order,
                        ServiceTypeId = request.ServiceTypeId,
                        ServiceType = serviceType,
                        PartId = request.PartId,
                        Part = part
                    };
                    order.Services.Add(service);
                    serviceType.Services.Add(service);
                    if(part != null) part.Service = service;
                    await _serviceRepository.AddServiceAsync(service);
                    return await Task.FromResult((true, "Service successfully created."));
                }
                else return await Task.FromResult((false, "Service type with given id does not exist."));
            }
            else return await Task.FromResult((false, "Order with given id does not exist."));
        }
        public async Task<(bool Confirmed, string Response)> UpdateService(UpdateServiceDto request, int id)
        {
            var service = await _serviceRepository.GetServiceById(id);
            if (service != null)
            {
                ApplicationUser? serviceman = null;
                Order? order = null;
                ServiceType? serviceType = null;
                Part? part = null;
                if (request.ServicemanId != null)
                {
                    serviceman = await _authRepository.GetUserByIdAsync(request.ServicemanId.Value);
                    if (serviceman == null) return await Task.FromResult((false, "Serviceman with given id does not exist."));
                    if (service.Serviceman != null) service.Serviceman.Services.Remove(service);
                    service.ServicemanId = request.ServicemanId;
                    service.Serviceman = serviceman;
                    serviceman!.Services.Add(service);
                }
                if (request.OrderId != null)
                {
                    order = await _orderRepository.GetOrderByIdAsync(request.OrderId.Value);
                    if (order == null) return await Task.FromResult((false, "Order with given id does not exist."));
                    order.Services.Remove(service);
                    service.OrderId = request.OrderId.Value;
                    service.Order = order;
                    order.Services.Add(service);
                }
                if (request.ServiceTypeId != null)
                {
                    serviceType = await _serviceTypeRepository.GetServiceTypeByIdAsync(request.ServiceTypeId.Value);
                    if (serviceType == null) return await Task.FromResult((false, "Service type with given id does not exist."));
                    serviceType.Services.Remove(service);
                    service.ServiceTypeId = request.ServiceTypeId.Value;
                    service.ServiceType = serviceType;
                    serviceType.Services.Add(service);
                }
                if (request.PartId != null)
                {
                    part = await _partRepository.GetPartByIdAsync(request.PartId.Value);
                    if (part == null) return await Task.FromResult((false, "Part with given id does not exist."));
                    if (service.Part != null) service.Part.Service = null;
                    service.PartId = request.PartId;
                    service.Part = part;
                    part!.Service = service;
                    service.PartPrice = part == null ? 0 : part.Model.Price;
                }
                if (request.ServicePrice != null) service.ServicePrice = request.ServicePrice.Value;
                if (request.Status != null) service.Status = request.Status.Value;
                if (request.Date != null) service.Date = request.Date.Value;
                if (request.Guarantee != null) service.Guarantee = request.Guarantee.Value;
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
