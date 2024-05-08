using EService.Data;
using EService.Models;

namespace EService.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly ApplicationDbContext _context;

        public ReviewRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Review?> GetReviewById(int id)
        {
            return await Task.Run(() => _context.Reviews.FindAsync(id).Result);

        }
        public async Task<List<Review>> GetAllReviews()
        {
            return await Task.Run(() => _context.Reviews.ToList());

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
