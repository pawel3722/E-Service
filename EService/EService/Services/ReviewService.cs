using EService.Dtos.ReviewDtos;
using EService.Models;
using EService.Repositories;

namespace EService.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        public ReviewService(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public async Task<Review?> GetReview(int id)
        {
            return await _reviewRepository.GetReviewById(id);

        }
        public async Task<List<Review>> GetAllReviews()
        {
            return await _reviewRepository.GetAllReviews();

        }
        public async Task<(bool Confirmed, string Response)> CreateReview(ReviewDto request)
        {
            var review = new Review
            {
                Rating = request.Rating,
                Comment = request.Comment,
                OrderId = request.OrderId,
                
            };
            await _reviewRepository.AddReviewAsync(review);
            return await Task.FromResult((true, "Review successfully created."));
        }
        public async Task<(bool Confirmed, string Response)> UpdateReview(ReviewDto request, int id)
        {
            var review = await _reviewRepository.GetReviewById(id);
            if (review != null)
            {
                review.Rating = request.Rating;
                review.Comment = request.Comment;
                review.OrderId = request.OrderId;
                await _reviewRepository.SaveChangesAsync();
                return await Task.FromResult((true, "Review successfully updated."));
            }
            else return await Task.FromResult((false, "Review with given id does not exist."));
        }
        public async Task<(bool Confirmed, string Response)> DeleteReview(int id)
        {
            var review = await _reviewRepository.GetReviewById(id);
            if (review != null)
            {
                await _reviewRepository.RemoveReviewAsync(review);
                return await Task.FromResult((true, "Review successfully deleted."));
            }
            else return await Task.FromResult((false, "Rerview with given id does not exist."));
        }

    }
}
