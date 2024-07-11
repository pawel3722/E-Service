using AutoMapper;
using EService.Dtos.ReviewDtos;
using EService.Dtos.ServiceDtos;
using EService.Models;
using EService.Repositories;
using EService.Repositories.Interfaces;
using System.Security.Claims;
using System.Transactions;

namespace EService.Services
{
    public class ServiceService : IServiceService
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IServiceTypeRepository _serviceTypeRepository;
        private readonly IPartRepository _partRepository;
        private readonly IApplicationUserRepository _applicationUserRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        public ServiceService(IServiceRepository serviceRepository, IOrderRepository orderRepository, IServiceTypeRepository serviceTypeRepository, IPartRepository partRepository, IApplicationUserRepository applicationUserRepository, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _serviceRepository = serviceRepository;
            _orderRepository = orderRepository;
            _serviceTypeRepository = serviceTypeRepository;
            _partRepository = partRepository;
            _applicationUserRepository = applicationUserRepository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }
        public async Task<ReturnServiceDto?> GetService(int id)
        {
            var review = await _serviceRepository.GetServiceById(id);
            return _mapper.Map<ReturnServiceDto>(review);
           // return await _serviceRepository.GetServiceById(id);
        }
        public async Task<List<ReturnServiceDto>> GetAllServices()
        {
            var review = await _serviceRepository.GetAllServices();
            return _mapper.Map<List<ReturnServiceDto>>(review);
           // return await _serviceRepository.GetAllServices();
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
                using var scope = new TransactionScope(TransactionScopeOption.Required,
                    new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                    TransactionScopeAsyncFlowOption.Enabled);
                try
                {
                    part = await _partRepository.GetPartByIdAsync(request.PartId.Value);
                    if (part == null) return await Task.FromResult((false, "Part with given id does not exist."));
                    if (part.Service != null) return await Task.FromResult((false, "Part is used in another service."));
                    var newService = new Service
                    {
                        Status = ServiceStatus.Created,
                        PartPrice = part == null ? 0 : part!.Model.Price,
                        ServicePrice = request.ServicePrice,
                        OrderId = request.OrderId,
                        Order = order,
                        ServiceTypeId = request.ServiceTypeId,
                        ServiceType = serviceType,
                        PartId = request.PartId,
                        Part = part
                    };
                    await _serviceRepository.AddServiceAsync(newService);
                    scope.Complete();
                    return await Task.FromResult((true, "Service successfully created."));
                }
                catch (Exception ex)
                {
                    return await Task.FromResult((false, "Error during processing request."));
                }
            }
            var service = new Service
            {
                Status = ServiceStatus.Created,
                PartPrice = part == null ? 0 : part!.Model.Price,
                ServicePrice = request.ServicePrice,
                OrderId = request.OrderId,
                Order = order,
                ServiceTypeId = request.ServiceTypeId,
                ServiceType = serviceType,
                PartId = request.PartId,
                Part = part
            };
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
            if(request.ServicePrice != null) service.ServicePrice = request.ServicePrice.Value;
            if(request.Status != null) service.Status = request.Status.Value;
            if(request.Date != null) service.Date = request.Date.Value;
            if(request.Guarantee != null) service.Guarantee = request.Guarantee.Value;
            if(request.OrderId != null)
            {
                order = await _orderRepository.GetOrderByIdAsync(request.OrderId.Value);
                if (order == null) return await Task.FromResult((false, "Order with given id does not exist."));
                service.OrderId = request.OrderId.Value;
                service.Order = order;
            }
            if (request.ServicemanId != null)
            {
                serviceman = await _applicationUserRepository.GetUserByIdAsync(request.ServicemanId.Value);
                if (serviceman == null) return await Task.FromResult((false, "Serviceman with given id does not exist."));
                service.ServicemanId = request.ServicemanId;
                service.Serviceman = serviceman;
                service.Status = ServiceStatus.AssignedWorker;
                var thisOrder = await _orderRepository.GetOrderByIdAsync(service.OrderId);
                if (thisOrder!.Status == OrderStatus.FinishedAnalysis)
                {
                    bool allAssigned = true;
                    foreach (var s in thisOrder!.Services)
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
            if (request.ServiceTypeId != null)
            {
                serviceType = await _serviceTypeRepository.GetServiceTypeByIdAsync(request.ServiceTypeId.Value);
                if (serviceType == null) return await Task.FromResult((false, "Service type with given id does not exist."));
                service.ServiceTypeId = request.ServiceTypeId.Value;
                service.ServiceType = serviceType;
            }
            if (request.PartId != null)
            {
                using var scope = new TransactionScope(TransactionScopeOption.Required,
                    new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                    TransactionScopeAsyncFlowOption.Enabled);
                try
                {
                    part = await _partRepository.GetPartByIdAsync(request.PartId.Value);
                    if (part == null) return await Task.FromResult((false, "Part with given id does not exist."));
                    if (part.Service != null) return await Task.FromResult((false, "Part is used in another service."));
                    part.Service = service;
                    double price = part.Model!.Price;
                    service.PartPrice = price;
                    await _serviceRepository.SaveChangesAsync();
                    scope.Complete();
                    return await Task.FromResult((true, "Service successfully updated."));
                }
                catch(Exception ex)
                {
                    return await Task.FromResult((false, "Error during processing request."));
                }
            }
            await _serviceRepository.SaveChangesAsync();
            return await Task.FromResult((true, "Service successfully updated."));
        }
        public async Task<(bool Confirmed, string Response)> UpdateServiceStatus(UpdateServiceDto request, int id)
        {
            var user = await _applicationUserRepository.GetUserByIdAsync(Int32.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!));
            if(user == null) return await Task.FromResult((false, "User does not exist."));
            var service = await _serviceRepository.GetServiceById(id);
            if(service == null) return await Task.FromResult((false, "Service with given id does not exist."));
            if(service.ServicemanId != user.Id) return await Task.FromResult((false, "Service does not belong to this serviceman."));
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
                if (allFinished) order.Status = OrderStatus.Finished;
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
