using EService.Dtos.OrderDtos;
using EService.Models;

namespace EService.Services
{
    public interface IOrderService
    {
        public Task<Order?> GetOrderAsync(int id);
        public Task<List<Order>> GetAllOrdersAsync();
        public Task<(bool Confirmed, string Response)> CreateOrderAsync(CreateOrderDto request);
        public Task<(bool Confirmed, string Response)> UpdateOrderAsync(UpdateOrderDto request, int id);
        public Task<(bool Confirmed, string Response)> DeleteOrderAsync(int id);
    }
}
