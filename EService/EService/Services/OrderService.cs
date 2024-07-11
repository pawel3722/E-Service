using AutoMapper;
using EService.Dtos.ApplicationUserDtos;
using EService.Dtos.OrderDtos;
using EService.Dtos.ReviewDtos;
using EService.Models;
using EService.Repositories;
using EService.Repositories.Interfaces;
using System.Security.Claims;
using System.Transactions;

namespace EService.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IServiceTypeRepository _serviceTypeRepository;
        private readonly IPartRepository _partRepository;
        private readonly IApplicationUserRepository _applicationUserRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository, IServiceTypeRepository serviceTypeRepository, IPartRepository partRepository, IApplicationUserRepository applicationUserRepository, IReviewRepository reviewRepository, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _serviceTypeRepository = serviceTypeRepository;
            _partRepository = partRepository;
            _applicationUserRepository = applicationUserRepository;
            _reviewRepository = reviewRepository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<ReturnOrderDto?> GetOrderAsync(int id)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);
            return _mapper.Map<ReturnOrderDto>(order);
            // await _orderRepository.GetOrderByIdAsync(id);
        }
        public async Task<List<ReturnOrderDto>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetAllOrdersAsync();
            return _mapper.Map<List<ReturnOrderDto>>(orders);
            //return await _orderRepository.GetAllOrdersAsync();
        }
        public async Task<(bool Confirmed, string Response)> CreateOrderAsync(CreateOrderDto request)
        {
            var customer = await _applicationUserRepository.GetUserByIdAsync(request.CustomerId);
            if(customer == null) return await Task.FromResult((false, "Customer with given id does not exist."));
            ApplicationUser? manager = null;
            if(request.ManagerId != null)
            {
                manager = await _applicationUserRepository.GetUserByIdAsync(request.ManagerId.Value);
                if(manager == null) return await Task.FromResult((false, "Manager with given id does not exist."));
            }
            var listOfServices = new List<Service>();
            if(request.Services.Count > 0)
            {
                using var scope = new TransactionScope(TransactionScopeOption.Required,
                    new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                    TransactionScopeAsyncFlowOption.Enabled);
                try
                {
                    foreach (var serviceDto in request.Services)
                    {
                        var serviceType = await _serviceTypeRepository.GetServiceTypeByIdAsync(serviceDto.ServiceTypeId);
                        if (serviceType == null) return await Task.FromResult((false, "Service type with given id does not exist."));
                        Part? part = null;
                        if (serviceDto.PartId != null)
                        {
                            part = await _partRepository.GetPartByIdAsync(serviceDto.PartId.Value);
                            if (part == null) return await Task.FromResult((false, "Part with given id does not exist."));
                            if (part.Service != null) return await Task.FromResult((false, "Part is used in another service."));
                        }
                        var service = new Service()
                        {
                            Status = ServiceStatus.Created,
                            ServicePrice = serviceDto.ServicePrice,
                            PartId = serviceDto.PartId,
                            Part = part,
                            PartPrice = part == null ? 0 : part.Model.Price,
                            ServiceTypeId = serviceDto.ServiceTypeId,
                            ServiceType = serviceType
                        };
                        listOfServices.Add(service);
                    }
                    var newOrder = new Order
                    {
                        Date = request.Date,
                        Status = (manager != null ? OrderStatus.AssignedManager : OrderStatus.StartedProcessing),
                        Paid = request.Paid,
                        CustomerId = request.CustomerId,
                        Customer = customer,
                        ManagerId = request.ManagerId,
                        Manager = manager,
                        Services = listOfServices
                    };
                    await _orderRepository.AddOrderAsync(newOrder);
                    scope.Complete();
                    return await Task.FromResult((true, "Order successfully created."));
                }
                catch (Exception ex)
                {
                    return await Task.FromResult((false, "Error during processing request."));
                }
            }
            var order = new Order
            {
                Date = request.Date,
                Status = (manager != null ? OrderStatus.AssignedManager : OrderStatus.StartedProcessing),
                Paid = request.Paid,
                CustomerId = request.CustomerId,
                Customer = customer,
                ManagerId = request.ManagerId,
                Manager = manager,
                Services = listOfServices
            };
            await _orderRepository.AddOrderAsync(order);
            return await Task.FromResult((true, "Order successfully created."));
        }
        public async Task<(bool Confirmed, string Response)> UpdateOrderAsync(UpdateOrderDto request, int id)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);
            if(order == null) return await Task.FromResult((false, "Order with given id does not exist."));
            ApplicationUser? customer = null;
            ApplicationUser? manager = null;

            //NOWE
            if (!(request.CustomerId != null && request.CustomerId != order.CustomerId
                || request.ManagerId != null && request.ManagerId != order.ManagerId
                || request.Date != null && request.Date != order.Date
                || request.Paid != null && request.Paid != order.Paid
                || (request.CreateReviewDto.Rating != null && request.CreateReviewDto.Rating != order.Review.Rating 
                    || request.CreateReviewDto.Comment != null && request.CreateReviewDto.Comment != order.Review.Comment
                    || request.CreateReviewDto.OrderId != null && request.CreateReviewDto.OrderId != order.Review.OrderId)
                || request.newServices.Count > 0))
            {
                return await Task.FromResult((false, "No fields to be updated."));
            }
            customer = await _applicationUserRepository.GetUserByIdAsync(request.CustomerId.Value);
            if (customer == null) return await Task.FromResult((false, "Customer with given id does not exist."));
            order.CustomerId = request.CustomerId.Value;
            order.Customer = customer;

            manager = await _applicationUserRepository.GetUserByIdAsync(request.ManagerId.Value);
            if (manager == null) return await Task.FromResult((false, "Manager with given id does not exist."));
            order.ManagerId = request.ManagerId.Value;
            order.Manager = manager;
            order.Status = OrderStatus.AssignedManager;

            order.Date = request.Date.Value;
            order.Paid = request.Paid.Value;

            order.Review = new Review()
            {
                Rating = request.CreateReviewDto.Rating,
                Comment = request.CreateReviewDto.Comment,
                Order = order,
                OrderId = order.Id
            };

            using var scope = new TransactionScope(TransactionScopeOption.Required,
                    new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                    TransactionScopeAsyncFlowOption.Enabled);
            var listOfServices = order.Services;
            try
            {
                foreach (var serviceDto in request.newServices)
                {
                    var serviceType = await _serviceTypeRepository.GetServiceTypeByIdAsync(serviceDto.ServiceTypeId);
                    if (serviceType == null) return await Task.FromResult((false, "Service type with given id does not exist."));
                    Part? part = null;
                    if (serviceDto.PartId != null)
                    {
                        part = await _partRepository.GetPartByIdAsync(serviceDto.PartId.Value);
                        if (part == null) return await Task.FromResult((false, "Part with given id does not exist."));
                        if (part.Service != null) return await Task.FromResult((false, "Part is used in another service."));
                    }
                    var newService = new Service()
                    {
                        Status = ServiceStatus.Created,
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
                }
                order.Services = listOfServices;
                await _orderRepository.SaveChangesAsync();
                scope.Complete();
                return await Task.FromResult((true, "Order successfully updated."));
            }
            catch (Exception ex)
            {
                return await Task.FromResult((false, "Error during processing request."));
            }
            await _orderRepository.SaveChangesAsync();
            return await Task.FromResult((true, "Order successfully updated."));
            //NOWE

            /*
            if (request.CustomerId != null)
            {
                customer = await _applicationUserRepository.GetUserByIdAsync(request.CustomerId.Value);
                if(customer == null) return await Task.FromResult((false, "Customer with given id does not exist."));
                order.CustomerId = request.CustomerId.Value;
                order.Customer = customer;
            }
            if(request.ManagerId != null)
            {
                manager = await _applicationUserRepository.GetUserByIdAsync(request.ManagerId.Value);
                if(manager == null) return await Task.FromResult((false, "Manager with given id does not exist."));
                order.ManagerId = request.ManagerId.Value;
                order.Manager = manager;
                order.Status = OrderStatus.AssignedManager;
            }
            if(request.Date != null) order.Date = request.Date.Value;
            if(request.Paid != null) order.Paid = request.Paid.Value;
            if(request.CreateReviewDto != null) order.Review = new Review()
            {
                Rating = request.CreateReviewDto.Rating,
                Comment = request.CreateReviewDto.Comment,
                Order = order,
                OrderId = order.Id
            };
            if (request.newServices.Count > 0)
            {
                using var scope = new TransactionScope(TransactionScopeOption.Required,
                    new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                    TransactionScopeAsyncFlowOption.Enabled);
                var listOfServices = order.Services;
                try
                {
                    foreach (var serviceDto in request.newServices)
                    {
                        var serviceType = await _serviceTypeRepository.GetServiceTypeByIdAsync(serviceDto.ServiceTypeId);
                        if (serviceType == null) return await Task.FromResult((false, "Service type with given id does not exist."));
                        Part? part = null;
                        if (serviceDto.PartId != null)
                        {
                            part = await _partRepository.GetPartByIdAsync(serviceDto.PartId.Value);
                            if (part == null) return await Task.FromResult((false, "Part with given id does not exist."));
                            if (part.Service != null) return await Task.FromResult((false, "Part is used in another service."));
                        }
                        var newService = new Service()
                        {
                            Status = ServiceStatus.Created,
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
                    }
                    order.Services = listOfServices;
                    await _orderRepository.SaveChangesAsync();
                    scope.Complete();
                    return await Task.FromResult((true, "Order successfully updated."));
                }
                catch (Exception ex)
                {
                    return await Task.FromResult((false, "Error during processing request."));
                }
            } 
            await _orderRepository.SaveChangesAsync();
            return await Task.FromResult((true, "Order successfully updated."));*/
        }
        public async Task<(bool Confirmed, string Response)> UpdateOrderServicesAsync(UpdateOrderDto request, int id)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);
            if (order == null) return await Task.FromResult((false, "Order with given id does not exist."));
            var listOfServices = order.Services;
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                    new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                    TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                foreach (var serviceDto in request.newServices)
                {
                    var serviceType = await _serviceTypeRepository.GetServiceTypeByIdAsync(serviceDto.ServiceTypeId);
                    if (serviceType == null) return await Task.FromResult((false, "Service type with given id does not exist."));
                    Part? part = null;
                    if (serviceDto.PartId != null)
                    {
                        part = await _partRepository.GetPartByIdAsync(serviceDto.PartId.Value);
                        if (part == null) return await Task.FromResult((false, "Part with given id does not exist."));
                        if (part.Service != null) return await Task.FromResult((false, "Part is used in another service."));
                    }
                    var newService = new Service()
                    {
                        Status = ServiceStatus.Created,
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
                }
                order.Services = listOfServices;
                await _orderRepository.SaveChangesAsync();
                scope.Complete();
                return await Task.FromResult((true, "Order successfully updated."));
            }
            catch (Exception ex)
            {
                return await Task.FromResult((false, "Error during processing request."));
            }
        }
        public async Task<(bool Confirmed, string Response)> UpdateOrderStatusAsync(UpdateOrderDto request, int id)
        {
            if(request.Status == null) return await Task.FromResult((false, "Bad request."));
            var user = await _applicationUserRepository.GetUserByIdAsync(Int32.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!));
            if(user == null) return await Task.FromResult((false, "User does not exist."));
            if(user!.Roles.Where(u => u.Name == "Manager").FirstOrDefault() == null && request.Status.Value != OrderStatus.Received) return await Task.FromResult((false, "Forbidden request."));
            if (user!.Roles.Where(u => u.Name == "Seller").FirstOrDefault() == null && request.Status.Value == OrderStatus.Received) return await Task.FromResult((false, "Forbidden request."));
            var order = await _orderRepository.GetOrderByIdAsync(id);
            if(order == null) return await Task.FromResult((false, "Order with given id does not exist."));
            if (!order.Paid && request.Status.Value == OrderStatus.Received) return await Task.FromResult((false, "There is no payment for this order."));
            order.Status = request.Status.Value;
            if(request.Status.Value == OrderStatus.FinishedAnalysis)
            {
                bool allAssigned = true;
                foreach (var service in order.Services)
                {
                    if (service.Status != ServiceStatus.AssignedWorker)
                    {
                        allAssigned = false;
                        break;
                    }
                }
                if (allAssigned) order.Status = OrderStatus.AssignedActions;
                else order.Status = OrderStatus.FinishedAnalysis;
            }
            await _orderRepository.SaveChangesAsync();
            return await Task.FromResult((true, "Order successfully updated."));
        }
        public async Task<(bool Confirmed, string Response)> UpdateOrderPaidSellerAsync(int id)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);
            if (order == null) return await Task.FromResult((false, "Order with given id does not exist."));
            order.Paid = true;
            await _orderRepository.SaveChangesAsync();
            return await Task.FromResult((true, "Order successfully updated."));
        }
        public async Task<(bool Confirmed, string Response)> UpdateSentReview(UpdateReviewDto request, int id)
        {
            var sendingUser = await _applicationUserRepository.GetUserByIdAsync(Int32.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!));
            if (sendingUser == null) return await Task.FromResult((false, "User with given id does not exist."));
            var review = await _reviewRepository.GetReviewByIdAsync(id);
            if (review == null) return await Task.FromResult((false, "Review with given id does not exist."));
            if (sendingUser.Id != review.Order.CustomerId) return await Task.FromResult((false, "Cannot update a review sent by a different user."));
            Order? order = null;


            //NOWE

            if (request.OrderId != null && request.OrderId != review.OrderId
                || request.Rating != null && request.Rating != review.Rating)
            {
                return await Task.FromResult((false, "No fields to be updated."));
            }
            order = await _orderRepository.GetOrderByIdAsync(request.OrderId.Value);
            if (order == null) return await Task.FromResult((false, "Order with given id does not exist."));
            review.OrderId = request.OrderId.Value;
            review.Order = order;
            order.Review = review;

            review.Rating = request.Rating.Value;

            await _reviewRepository.SaveChangesAsync();
            return await Task.FromResult((true, "Review successfully updated."));
            //NOWE


            /*if (request.OrderId != null)
            {
                order = await _orderRepository.GetOrderByIdAsync(request.OrderId.Value);
                if (order == null) return await Task.FromResult((false, "Order with given id does not exist."));
                review.OrderId = request.OrderId.Value;
                review.Order = order;
                order.Review = review;
            }
            if(request.Rating != null) review.Rating = request.Rating.Value;
            await _reviewRepository.SaveChangesAsync();
            return await Task.FromResult((true, "Review successfully updated.")); */

        }
        public async Task<(bool Confirmed, string Response)> DeleteOrderAsync(int id)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);
            if(order == null) return await Task.FromResult((false, "Order with given id does not exist."));
            await _orderRepository.RemoveOrderAsync(order);
            return await Task.FromResult((true, "Order successfully deleted."));
        }
        public async Task<(bool Confirmed, string Response)> DeleteSentReview(int id)
        {
            var sendingUser = await _applicationUserRepository.GetUserByIdAsync(Int32.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!));
            if(sendingUser == null) return await Task.FromResult((false, "User with given id does not exist."));
            var review = await _reviewRepository.GetReviewByIdAsync(id);
            if(review == null) return await Task.FromResult((false, "Review with given id does not exist."));
            if(sendingUser.Id != review.Order.CustomerId) return await Task.FromResult((false, "Cannot delete a review sent by a different user."));
            await _reviewRepository.RemoveReviewAsync(review);
            return await Task.FromResult((true, "Review successfully deleted."));
        }
        public async Task<(bool Confirmed, string Response, ReturnReviewDto? Review)> GetReviewFromOrderAsync(int id)
        {         

            var user = await _applicationUserRepository.GetUserByIdAsync(Int32.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!));
            if(user == null) return await Task.FromResult<(bool Confirmed, string Response, ReturnReviewDto? Review)>((false, "User with given id does not exist.", null));
            var order = await _orderRepository.GetOrderByIdAsync(id);
            if(order == null) return await Task.FromResult<(bool Confirmed, string Response, ReturnReviewDto? Review)>((false, "Order with given id does not exist.", null));
            if(order.Review == null) return await Task.FromResult<(bool Confirmed, string Response, ReturnReviewDto? Review)>((false, "Review for this order does not exist.", null));
            if (user.Id != order.CustomerId) return await Task.FromResult<(bool Confirmed, string Response, ReturnReviewDto? Review)>((false, "Order with given id does not belong to this user.", null));
            var reviewDto = _mapper.Map<ReturnReviewDto>(order.Review);
            return await Task.FromResult((true, "", reviewDto));
        }
    }
}
