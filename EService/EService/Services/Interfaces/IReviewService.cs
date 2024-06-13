using EService.Dtos.ReviewDtos;
using EService.Models;

namespace EService.Services
{
    public interface IReviewService
    {
        public Task<ReturnReviewDto?> GetReviewAsync(int id);
        public Task<List<ReturnReviewDto>> GetAllReviewsAsync();
        public Task<(bool Confirmed, string Response)> CreateReviewAsync(CreateReviewDto request);
        public Task<(bool Confirmed, string Response)> UpdateReview(UpdateReviewDto request, int id);
        public Task<(bool Confirmed, string Response)> DeleteReview(int id);
    }
}
