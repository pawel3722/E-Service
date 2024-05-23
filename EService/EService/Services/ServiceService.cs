using EService.Dtos.ServiceDtos;
using EService.Models;
using EService.Repositories.Interfaces;
using System.Security.Claims;

namespace EService.Services
{
    public class ServiceService : IServiceService
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IServiceTypeRepository _serviceTypeRepository;
        private readonly IPartRepository _partRepository;
        private readonly IAuthRepository _authRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ServiceService(IServiceRepository serviceRepository, IOrderRepository orderRepository, IServiceTypeRepository serviceTypeRepository, IPartRepository partRepository, IAuthRepository authRepository, IHttpContextAccessor httpContextAccessor)
        {
            _serviceRepository = serviceRepository;
            _orderRepository = orderRepository;
            _serviceTypeRepository = serviceTypeRepository;
            _partRepository = partRepository;
            _authRepository = authRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Service?> GetService(int id)
        {
            return await _serviceRepository.GetServiceById(id);
        }
        public async Task<List<Service>> GetAllServices()
        {
            return await _serviceRepository.GetAllServices();
        }
        public async Task<(bool Confirmed, string Response, List<Service>? Services)> GetClientServices(int? orderId)
        {
            var user = await _authRepository.GetUserByIdAsync(Int32.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!));
            if(user == null) return await Task.FromResult<(bool, string, List<Service>?)>((false, "User not found.", null));
            if(orderId == null)
            {
                var services = new List<Service>();
                foreach(var customerOrder in user.CustomerOrders)
                {
                    services.AddRange(customerOrder.Services);
                }
                return await Task.FromResult((true, "", services));
            }
            var order = await _orderRepository.GetOrderByIdAsync(orderId.Value);
            if(order == null) return await Task.FromResult<(bool, string, List<Service>?)>((false, "Order with given id does not exist.", null));
            if(order.CustomerId != user.Id) return await Task.FromResult<(bool, string, List<Service>?)>((false, "Order with given id was not made by the user.", null));
            return await Task.FromResult((true, "", order.Services));
        }
        public async Task<(bool Confirmed, string Response, List<Service>? Services)> GetServicemanServices(int? orderId)
        {
            var user = await _authRepository.GetUserByIdAsync(Int32.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!));
            if (user == null) return await Task.FromResult<(bool, string, List<Service>?)>((false, "User not found.", null));
            if (orderId == null) return await Task.FromResult((true, "", user.Services));
            var order = await _orderRepository.GetOrderByIdAsync(orderId.Value);
            if (order == null) return await Task.FromResult<(bool, string, List<Service>?)>((false, "Order with given id does not exist.", null));
            if (order.CustomerId != user.Id) return await Task.FromResult<(bool, string, List<Service>?)>((false, "Order with given id was not made by the user.", null));
            return await Task.FromResult((true, "", order.Services));
        }
        public async Task<(bool Confirmed, string Response)> CreateService(CreateServiceDto request)
        {
            var order = await _orderRepository.GetOrderByIdAsync(request.OrderId);
            if(order == null) return await Task.FromResult((false, "Order with given id does not exist."));
            var serviceType = await _serviceTypeRepository.GetServiceTypeByIdAsync(request.ServiceTypeId);
            if(serviceType == null) return await Task.FromResult((false, "Service type with given id does not exist."));
            Part? part = null;
            if (request.PartId != null)
            {
                part = await _partRepository.GetPartByIdAsync(request.PartId.Value);
                if(part == null) return await Task.FromResult((false, "Part with given id does not exist."));
            }
            var service = new Service
            {
                Status = ServiceStatus.Created,
                PartPrice = (part == null ? 0 : part.Model.Price),
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
        public async Task<(bool Confirmed, string Response)> UpdateService(UpdateServiceDto request, int id)
        {
            var service = await _serviceRepository.GetServiceById(id);
            if(service == null) return await Task.FromResult((false, "Service with given id does not exist."));
            ApplicationUser? serviceman = null;
            Order? order = null;
            ServiceType? serviceType = null;
            Part? part = null;
            if (request.OrderId != null)
            {
                order = await _orderRepository.GetOrderByIdAsync(request.OrderId.Value);
                if (order == null) return await Task.FromResult((false, "Order with given id does not exist."));
                order.Services.Remove(service);
                service.OrderId = request.OrderId.Value;
                service.Order = order;
                order.Services.Add(service);
            }
            if (request.ServicemanId != null)
            {
                serviceman = await _authRepository.GetUserByIdAsync(request.ServicemanId.Value);
                if(serviceman == null) return await Task.FromResult((false, "Serviceman with given id does not exist."));
                if(service.Serviceman != null) service.Serviceman.Services.Remove(service);
                service.ServicemanId = request.ServicemanId;
                service.Serviceman = serviceman;
                service.Status = ServiceStatus.AssignedWorker;
                serviceman!.Services.Add(service);
                var thisOrder = await _orderRepository.GetOrderByIdAsync(service.OrderId);
                if (thisOrder!.Status == OrderStatus.FinishedAnalysis)
                {
                    bool allAssigned = true;
                    foreach (var s in service.Order.Services)
                    {
                        if (s.Status != ServiceStatus.AssignedWorker)
                        {
                            allAssigned = false;
                            break;
                        }
                    }
                    if (allAssigned) service.Order.Status = OrderStatus.AssignedActions;
                }
            }
            if(request.ServiceTypeId != null)
            {
                serviceType = await _serviceTypeRepository.GetServiceTypeByIdAsync(request.ServiceTypeId.Value);
                if (serviceType == null) return await Task.FromResult((false, "Service type with given id does not exist."));
                serviceType.Services.Remove(service);
                service.ServiceTypeId = request.ServiceTypeId.Value;
                service.ServiceType = serviceType;
                serviceType.Services.Add(service);
            }
            if(request.PartId != null)
            {
                part = await _partRepository.GetPartByIdAsync(request.PartId.Value);
                if (part == null) return await Task.FromResult((false, "Part with given id does not exist."));
                if (service.Part != null) service.Part.Service = null;
                service.PartId = request.PartId;
                service.Part = part;
                part!.Service = service;
                service.PartPrice = (part == null ? 0 : part.Model.Price);
            }
            if(request.ServicePrice != null) service.ServicePrice = request.ServicePrice.Value;
            if(request.Status != null) service.Status = request.Status.Value;
            if(request.Date != null) service.Date = request.Date.Value;
            if(request.Guarantee != null) service.Guarantee = request.Guarantee.Value;
            await _serviceRepository.SaveChangesAsync();
            return await Task.FromResult((true, "Service successfully updated."));
        }
        public async Task<(bool Confirmed, string Response)> UpdateServiceStatus(UpdateServiceDto request, int id)
        {
            var service = await _serviceRepository.GetServiceById(id);
            if(service == null) return await Task.FromResult((false, "Service with given id does not exist."));
            if(request.Status != null && request.Status > ServiceStatus.AssignedWorker) service.Status = request.Status.Value;
            if(service.Status == ServiceStatus.Finished)
            {
                bool allFinished = true;
                Order order = (await _orderRepository.GetOrderByIdAsync(service.OrderId))!;
                foreach(var orderService in order.Services)
                {
                    if(orderService.Status != ServiceStatus.Finished)
                    {
                        allFinished = false;
                        break;
                    }
                }
                if(allFinished) order.Status = OrderStatus.Finished;
            }
            if(request.Date != null) service.Date = request.Date.Value;
            if(request.Guarantee != null) service.Guarantee = request.Guarantee.Value;
            await _serviceRepository.SaveChangesAsync();
            return await Task.FromResult((true, "Service successfully updated."));
        }
        public async Task <(bool Confirmed, string Response)> DeleteService(int id)
        {
            var service = await _serviceRepository.GetServiceById(id);
            if(service == null) return await Task.FromResult((false, "Service with given id does not exist."));
            await _serviceRepository.RemoveServiceAsync(service);
            return await Task.FromResult((true, "Service successfully deleted."));
        }
    }
}
