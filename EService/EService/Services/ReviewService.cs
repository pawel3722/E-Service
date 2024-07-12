using AutoMapper;
using EService.Dtos.ApplicationUserDtos;
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
        private readonly IMapper _mapper;
        public ReviewService(IReviewRepository reviewRepository, IOrderRepository orderRepository, IMapper mapper)
        {
            _reviewRepository = reviewRepository;
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<ReturnReviewDto?> GetReviewAsync(int id)
        {
            var reviews = await _reviewRepository.GetReviewByIdAsync(id);
            return _mapper.Map<ReturnReviewDto>(reviews);
        }
        public async Task<List<ReturnReviewDto>> GetAllReviewsAsync()
        {
            var review = await _reviewRepository.GetAllReviewsAsync();
            return _mapper.Map<List<ReturnReviewDto>>(review);
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
            if (!(request.OrderId != null && request.OrderId != review.OrderId
                || request.Rating != null && request.Rating != review.Rating))
                return await Task.FromResult((false, "No fields to be updated."));
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
            if (review == null) return await Task.FromResult((false, "Review with given id does not exist."));
            await _reviewRepository.RemoveReviewAsync(review);
            return await Task.FromResult((true, "Review successfully deleted."));
        }
    }
}
