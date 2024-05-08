using EService.Dtos.ReviewDtos;
using EService.Models;

namespace EService.Services
{
    public interface IReviewService
    {
        public Task<Review?> GetReview(int id);
        public Task<List<Review>> GetAllReviews();
        public Task<(bool Confirmed, string Response)> CreateReview(ReviewDto request);
        public Task<(bool Confirmed, string Response)> UpdateReview(ReviewDto request, int id);
        public Task<(bool Confirmed, string Response)> DeleteReview(int id);
    }
}
