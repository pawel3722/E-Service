using EService.Data;
using EService.Models;
using EService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EService.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            return await Task.Run(() => _context.Orders.FirstOrDefaultAsync(o => o.Id == id));
        }
        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await Task.Run(() => _context.Orders.Include(o => o.Services).ToListAsync());
        }
        public async Task<List<Order>> GetCustomerOrdersAsync(int customerId)
        {
            return await Task.Run(() => _context.Orders.Where(o => o.CustomerId == customerId).ToListAsync());
        }
        public async Task<List<Order>> GetManagerOrdersAsync(int managerId)
        {
            return await Task.Run(() => _context.Orders.Where(o => o.ManagerId == managerId).ToListAsync());
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
