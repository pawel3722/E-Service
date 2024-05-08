using EService.Models;

namespace EService.Repositories
{
    public interface IOrderRepository
    {
        public Task<Order?> GetOrderById(int id);
        public Task<List<Order>> GetAllOrders();
        public Task AddOrderAsync(Order Order);
        public Task RemoveOrderAsync(Order Order);
        public Task SaveChangesAsync();
    }
}
