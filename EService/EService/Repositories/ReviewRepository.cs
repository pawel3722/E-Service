using EService.Data;
using EService.Models;
using EService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

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
            return await Task.Run(() => _context.Reviews.FirstOrDefaultAsync(r => r.Id == id));

        }
        public async Task<List<Review>> GetAllReviewsAsync()
        {
            return await Task.Run(() => _context.Reviews.ToListAsync());

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
