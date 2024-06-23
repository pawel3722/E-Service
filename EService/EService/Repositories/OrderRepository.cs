using EService.Data;
using EService.Models;
using EService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

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
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                                    new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                                    TransactionScopeAsyncFlowOption.Enabled);
            Order? order = null;
            try
            {
                order = await Task.Run(() => _context.Orders.
                    Include(o => o.Customer).
                    Include(o => o.Manager).
                    Include(o => o.Review).
                    Include(o => o.Services).
                    FirstOrDefaultAsync(o => o.Id == id));
                scope.Complete();
            }
            catch (Exception) { }
            return await Task.Run(() => order);
        }
        public async Task<List<Order>> GetAllOrdersAsync()
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                                   new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                                   TransactionScopeAsyncFlowOption.Enabled);
            List<Order> arr = new List<Order>();
            try
            {
                arr = await Task.Run(() => _context.Orders.
                    Include(o => o.Customer).
                    Include(o => o.Manager).
                    Include(o => o.Review).
                    Include(o => o.Services).
                    ToListAsync());
                scope.Complete();
            }
            catch (Exception) { }
            return await Task.Run(() => arr);
        }
        public async Task<List<Order>> GetCustomerOrdersAsync(int customerId)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                                   new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                                   TransactionScopeAsyncFlowOption.Enabled);
            List<Order> arr = new List<Order>();
            try
            {
                arr = await Task.Run(() => _context.Orders.Where(o => o.CustomerId == customerId).
                    Include(o => o.Customer).
                    Include(o => o.Manager).
                    Include(o => o.Review).
                    Include(o => o.Services).ToListAsync());
                scope.Complete();
            }
            catch (Exception) { }
            return await Task.Run(() => arr);
        }
        public async Task<List<Order>> GetManagerOrdersAsync(int managerId)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                                   new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                                   TransactionScopeAsyncFlowOption.Enabled);
            List<Order> arr = new List<Order>();
            try
            {
                arr = await Task.Run(() => _context.Orders.Where(o => o.ManagerId == managerId).
                    Include(o => o.Customer).
                    Include(o => o.Manager).
                    Include(o => o.Review).
                    Include(o => o.Services).
                    ToListAsync());
                scope.Complete();
            }
            catch (Exception) { }
            return await Task.Run(() => arr);
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
