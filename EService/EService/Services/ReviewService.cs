using EService.Dtos.ReviewDtos;
using EService.Models;
using EService.Repositories;
using EService.Repositories.Interfaces;
using System.Security.Claims;

namespace EService.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IAuthRepository _authRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ReviewService(IReviewRepository reviewRepository, IOrderRepository orderRepository, IAuthRepository authRepository, IHttpContextAccessor httpContextAccessor)
        {
            _reviewRepository = reviewRepository;
            _orderRepository = orderRepository;
            _authRepository = authRepository;
            _httpContextAccessor = httpContextAccessor;
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
            if (order == null) return await Task.FromResult((false, "Order with given id does not exist."));
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
        public async Task<(bool Confirmed, string Response)> UpdateReview(UpdateReviewDto request, int id)
        {
            var review = await _reviewRepository.GetReviewByIdAsync(id);
            if (review == null) return await Task.FromResult((false, "Review with given id does not exist."));
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
        public async Task<(bool Confirmed, string Response)> UpdateSentReview(UpdateReviewDto request, int id)
        {
            var sendingUser = await _authRepository.GetUserByIdAsync(Int32.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!));
            if (sendingUser == null) return await Task.FromResult((false, "User with given id does not exist."));
            var review = await _reviewRepository.GetReviewByIdAsync(id);
            if (review == null) return await Task.FromResult((false, "Review with given id does not exist."));
            if (sendingUser.Id != review.Order.CustomerId) return await Task.FromResult((false, "Cannot update a review sent by a different user."));
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
        public async Task<(bool Confirmed, string Response)> DeleteReview(int id)
        {
            var review = await _reviewRepository.GetReviewByIdAsync(id);
            if (review == null) return await Task.FromResult((false, "Rerview with given id does not exist."));
            await _reviewRepository.RemoveReviewAsync(review);
            return await Task.FromResult((true, "Review successfully deleted."));
        }
        public async Task<(bool Confirmed, string Response)> DeleteSentReview(int id)
        {
            var sendingUser = await _authRepository.GetUserByIdAsync(Int32.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!));
            if (sendingUser == null) return await Task.FromResult((false, "User with given id does not exist."));
            var review = await _reviewRepository.GetReviewByIdAsync(id);
            if (review == null) return await Task.FromResult((false, "Rerview with given id does not exist."));
            if (sendingUser.Id != review.Order.CustomerId) return await Task.FromResult((false, "Cannot delete a review sent by a different user."));
            await _reviewRepository.RemoveReviewAsync(review);
            return await Task.FromResult((true, "Review successfully deleted."));
        }

    }
}
