using EService.Dtos.ReviewDtos;
using EService.Models;
using EService.Repositories.Interfaces;

namespace EService.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IOrderRepository _orderRepository;
        public ReviewService(IReviewRepository reviewRepository, IOrderRepository orderRepository)
        {
            _reviewRepository = reviewRepository;
            _orderRepository = orderRepository;
        }

        public async Task<Review?> GetReviewAsync(int id)
        {
            return await _reviewRepository.GetReviewByIdAsync(id);
        }
        public async Task<List<Review>> GetAllReviewsAsync()
        {
            return await _reviewRepository.GetAllReviewsAsync();
        }
        public async Task<(bool Confirmed, string Response)> CreateReviewAsync(CreateReviewDto request)
        {
            var order = await _orderRepository.GetOrderByIdAsync(request.OrderId);
            if (order != null)
            {
                var review = new Review
                {
                    Rating = request.Rating,
                    Comment = request.Comment,
                    OrderId = request.OrderId,
                    Order = order
                };
                order.Review = review;
                await _reviewRepository.AddReviewAsync(review);
                return await Task.FromResult((true, "Review successfully created."));
            }
            else return await Task.FromResult((false, "Order with given id does not exist."));
        }
        public async Task<(bool Confirmed, string Response)> UpdateReview(UpdateReviewDto request, int id)
        {
            var review = await _reviewRepository.GetReviewByIdAsync(id);
            if (review != null)
            {
                Order? order = null;
                if (request.OrderId != null)
                {
                    order = await _orderRepository.GetOrderByIdAsync(request.OrderId.Value);
                    if (order == null) return await Task.FromResult((false, "Order with given id does not exist."));
                    review.OrderId = request.OrderId.Value;
                    review.Order = order;
                    order.Review = review;
                }
                if (request.Rating != null) review.Rating = request.Rating.Value;
                await _reviewRepository.SaveChangesAsync();
                return await Task.FromResult((true, "Review successfully updated."));
            }
            else return await Task.FromResult((false, "Review with given id does not exist."));
        }
        public async Task<(bool Confirmed, string Response)> DeleteReview(int id)
        {
            var review = await _reviewRepository.GetReviewByIdAsync(id);
            if (review != null)
            {
                await _reviewRepository.RemoveReviewAsync(review);
                return await Task.FromResult((true, "Review successfully deleted."));
            }
            else return await Task.FromResult((false, "Rerview with given id does not exist."));
        }

    }
}
