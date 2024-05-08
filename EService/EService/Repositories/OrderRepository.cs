using EService.Data;
using EService.Models;

namespace EService.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Order?> GetOrderById(int id)
        {
            return await Task.Run(() => _context.Orders.FindAsync(id).Result);

        }
        public async Task<List<Order>> GetAllOrders()
        {
            return await Task.Run(() => _context.Orders.ToList());

        }
        public async Task AddOrderAsync(Order order)
        {
            await _context.AddAsync(order);
            await SaveChangesAsync();
        }
        public async Task RemoveOrderAsync(Order order)
        {
            _context.Remove(order);
            await SaveChangesAsync();
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
