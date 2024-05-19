using EService.Dtos.ReviewDtos;
using EService.Models;

namespace EService.Services
{
    public interface IReviewService
    {
        public Task<Review?> GetReviewAsync(int id);
        public Task<List<Review>> GetAllReviewsAsync();
        public Task<(bool Confirmed, string Response)> CreateReviewAsync(CreateReviewDto request);
        public Task<(bool Confirmed, string Response)> UpdateReview(UpdateReviewDto request, int id);
        public Task<(bool Confirmed, string Response)> UpdateSentReview(UpdateReviewDto request, int id);
        public Task<(bool Confirmed, string Response)> DeleteReview(int id);
        public Task<(bool Confirmed, string Response)> DeleteSentReview(int id);
    }
}
