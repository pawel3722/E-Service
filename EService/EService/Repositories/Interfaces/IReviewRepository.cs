using EService.Models;

namespace EService.Repositories.Interfaces
{
    public interface IReviewRepository
    {
        public Task<Review?> GetReviewByIdAsync(int id);
        public Task<List<Review>> GetAllReviewsAsync();
        public Task AddReviewAsync(Review Review);
        public Task RemoveReviewAsync(Review Review);
        public Task SaveChangesAsync();
    }
}
