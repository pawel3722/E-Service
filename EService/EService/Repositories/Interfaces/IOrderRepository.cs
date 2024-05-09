using EService.Models;

namespace EService.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        public Task<Order?> GetOrderByIdAsync(int id);
        public Task<List<Order>> GetAllOrdersAsync();
        public Task AddOrderAsync(Order Order);
        public Task RemoveOrderAsync(Order Order);
        public Task SaveChangesAsync();
    }
}
