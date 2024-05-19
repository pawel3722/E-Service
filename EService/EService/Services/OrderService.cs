using EService.Dtos.OrderDtos;
using EService.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net.NetworkInformation;
using EService.Repositories.Interfaces;

namespace EService.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IAuthRepository _authRepository;
        private readonly IServiceTypeRepository _serviceTypeRepository;
        private readonly IPartRepository _partRepository;
        public OrderService(IOrderRepository orderRepository, IAuthRepository authRepository, IServiceTypeRepository serviceTypeRepository, IPartRepository partRepository)
        {
            _orderRepository = orderRepository;
            _authRepository = authRepository;
            _serviceTypeRepository = serviceTypeRepository;
            _partRepository = partRepository;
        }

        public async Task<Order?> GetOrderAsync(int id)
        {
            return await _orderRepository.GetOrderByIdAsync(id);
        }
        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _orderRepository.GetAllOrdersAsync();
        }
        public async Task<(bool Confirmed, string Response)> CreateOrderAsync(CreateOrderDto request)
        {
            var customer = await _authRepository.GetUserByIdAsync(request.CustomerId);
            if(customer != null)
            {
                ApplicationUser? manager = null;
                if(request.ManagerId != null)
                    manager = await _authRepository.GetUserByIdAsync(request.ManagerId.Value);
                var listOfServices = new List<Service>();
                foreach(var serviceDto in request.Services)
                {
                    Part? part = null;
                    if(serviceDto.PartId != null) 
                        part = await _partRepository.GetPartByIdAsync(serviceDto.PartId.Value);
                    var serviceType = await _serviceTypeRepository.GetServiceTypeByIdAsync(serviceDto.ServiceTypeId);
                    if (serviceType != null)
                    {
                        var newService = new Service()
                        {
                            Status = serviceDto.Status,
                            ServicePrice = serviceDto.ServicePrice,
                            PartId = serviceDto.PartId,
                            Part = part,
                            PartPrice = part == null ? 0 : part.Model.Price,
                            ServiceTypeId = serviceDto.ServiceTypeId,
                            ServiceType = serviceType
                        };
                        listOfServices.Add(newService);
                        serviceType.Services.Add(newService);
                    }
                    else return await Task.FromResult((false, "Service type with given id does not exist."));
                }
                var order = new Order
                {
                    Date = request.Date,
                    Status = request.Status,
                    Paid = request.Paid,
                    CustomerId = request.CustomerId,
                    Customer = customer,
                    ManagerId = request.ManagerId,
                    Manager = manager,
                    Services = listOfServices
                };
                customer.CustomerOrders.Add(order);
                if(manager != null) manager.ManagerOrders.Add(order);
                await _orderRepository.AddOrderAsync(order);
                order = await _orderRepository.GetOrderByIdAsync(order.Id);
                foreach(var service in order!.Services)
                {
                    service.OrderId = order.Id;
                    service.Order = order;
                }
                await _orderRepository.SaveChangesAsync();
                return await Task.FromResult((true, "Order successfully created."));
            }
            else return await Task.FromResult((false, "Customer with given id does not exist."));
        }
        public async Task<(bool Confirmed, string Response)> UpdateOrderAsync(UpdateOrderDto request, int id)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);
            if(order != null)
            {
                ApplicationUser? customer = null;
                ApplicationUser? manager = null;
                if(request.CustomerId != null)
                {
                    customer = await _authRepository.GetUserByIdAsync(request.CustomerId.Value);
                    if(customer == null) return await Task.FromResult((false, "Customer with given id does not exist."));
                    order.Customer.CustomerOrders.Remove(order);
                    order.CustomerId = request.CustomerId.Value;
                    order.Customer = customer;
                }
                if(request.ManagerId != null)
                {
                    manager = await _authRepository.GetUserByIdAsync(request.ManagerId.Value);
                    if(manager == null) return await Task.FromResult((false, "Manager with given id does not exist."));
                    if(order.Manager != null) order.Manager.ManagerOrders.Remove(order);
                    order.ManagerId = request.ManagerId.Value;
                    order.Manager = manager;
                }
                if(request.Date != null) order.Date = request.Date.Value;
                if(request.Status != null) order.Status = request.Status.Value;
                if(request.Paid != null) order.Paid = request.Paid.Value;
                if(request.CreateReviewDto != null) order.Review = new Review()
                {
                    Rating = request.CreateReviewDto.Rating,
                    Comment = request.CreateReviewDto.Comment,
                    Order = order,
                    OrderId = order.Id
                };
                var listOfServices = new List<Service>();
                foreach (var serviceDto in request.newServices)
                {
                    Part? part = null;
                    if (serviceDto.PartId != null)
                        part = await _partRepository.GetPartByIdAsync(serviceDto.PartId.Value);
                    var serviceType = await _serviceTypeRepository.GetServiceTypeByIdAsync(serviceDto.ServiceTypeId);
                    if (serviceType != null)
                    {
                        var newService = new Service()
                        {
                            Status = serviceDto.Status,
                            ServicePrice = serviceDto.ServicePrice,
                            PartId = serviceDto.PartId,
                            Part = part,
                            PartPrice = part == null ? 0 : part.Model.Price,
                            ServiceTypeId = serviceDto.ServiceTypeId,
                            ServiceType = serviceType,
                            OrderId = order.Id,
                            Order = order
                        };
                        listOfServices.Add(newService);
                        serviceType.Services.Add(newService);
                    }
                    else return await Task.FromResult((false, "Service type with given id does not exist."));
                }
                await _orderRepository.SaveChangesAsync();
                return await Task.FromResult((true, "Order successfully updated."));
            }
            else return await Task.FromResult((false, "Order with given id does not exist."));
        }
        public async Task<(bool Confirmed, string Response)> DeleteOrderAsync(int id)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);
            if (order != null)
            {
                await _orderRepository.RemoveOrderAsync(order);
                return await Task.FromResult((true, "Order successfully deleted."));
            }
            else return await Task.FromResult((false, "Order with given id does not exist."));
        }
    }
}
