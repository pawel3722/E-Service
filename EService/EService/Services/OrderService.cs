using EService.Repositories;
using EService.Dtos.OrderDtos;
using EService.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net.NetworkInformation;

namespace EService.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Order?> GetOrder(int id)
        {
            return await _orderRepository.GetOrderById(id);

        }
        public async Task<List<Order>> GetAllOrders()
        {
            return await _orderRepository.GetAllOrders();

        }
        public async Task<(bool Confirmed, string Response)> CreateOrder(OrderDto request)
        {
            var order = new Order
            {
                Date = request.Date,
                Status = request.Status,
                Paid = request.Paid,
                CustomerId = request.CustomerId,
                //Services = request.Services
            };
            await _orderRepository.AddOrderAsync(order);
            return await Task.FromResult((true, "Order successfully created."));
        }
        public async Task<(bool Confirmed, string Response)> UpdateOrder(OrderDto request, int id)
        {
            var order = await _orderRepository.GetOrderById(id);
            if (order != null)
            {
                order.Date = request.Date;
                order.Status = request.Status;
                order.Paid = request.Paid;
                order.CustomerId = request.CustomerId;
                //order.Services = request.Services;
                await _orderRepository.SaveChangesAsync();
                return await Task.FromResult((true, "Order successfully updated."));
            }
            else return await Task.FromResult((false, "Order with given id does not exist."));
        }
        public async Task<(bool Confirmed, string Response)> DeleteOrder(int id)
        {
            var order = await _orderRepository.GetOrderById(id);
            if (order != null)
            {
                await _orderRepository.RemoveOrderAsync(order);
                return await Task.FromResult((true, "Order successfully deleted."));
            }
            else return await Task.FromResult((false, "Order with given id does not exist."));
        }
    }
}
