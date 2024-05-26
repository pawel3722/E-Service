using EService.Data;
using EService.Models;
using EService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace EService.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly ApplicationDbContext _context;

        public ReviewRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Review?> GetReviewByIdAsync(int id)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                                   new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                                   TransactionScopeAsyncFlowOption.Enabled);
            Review? review = null;
            try
            {
                review = await Task.Run(() => _context.Reviews.FirstOrDefaultAsync(r => r.Id == id));
                scope.Complete();
            }
            catch (Exception) { }
            return await Task.Run(() => review);
        }
        public async Task<List<Review>> GetAllReviewsAsync()
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                                   new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                                   TransactionScopeAsyncFlowOption.Enabled);
            List<Review> arr = new List<Review>();
            try
            {
                arr = await Task.Run(() => _context.Reviews.ToListAsync());
                scope.Complete();
            }
            catch (Exception) { }
            return await Task.Run(() => arr);
        }
        public async Task AddReviewAsync(Review review)
        {
            await _context.AddAsync(review);
            await SaveChangesAsync();
        }
        public async Task RemoveReviewAsync(Review review)
        {
            _context.Remove(review);
            await SaveChangesAsync();
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
