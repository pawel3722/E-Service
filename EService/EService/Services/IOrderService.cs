using EService.Dtos.OrderDtos;
using EService.Models;

namespace EService.Services
{
    public interface IOrderService
    {
        public Task<Order?> GetOrder(int id);
        public Task<List<Order>> GetAllOrders();
        public Task<(bool Confirmed, string Response)> CreateOrder(OrderDto request);
        public Task<(bool Confirmed, string Response)> UpdateOrder(OrderDto request, int id);
        public Task<(bool Confirmed, string Response)> DeleteOrder(int id);
    }
}
