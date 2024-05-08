using EService.Models;

namespace EService.Repositories
{
    public interface IReviewRepository
    {
        public Task<Review?> GetReviewById(int id);
        public Task<List<Review>> GetAllReviews();
        public Task AddReviewAsync(Review Review);
        public Task RemoveReviewAsync(Review Review);
        public Task SaveChangesAsync();
    }
}
