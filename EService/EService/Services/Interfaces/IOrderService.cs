using EService.Dtos.OrderDtos;
using EService.Dtos.ReviewDtos;
using EService.Models;

namespace EService.Services
{
    public interface IOrderService
    {
        public Task<Order?> GetOrderAsync(int id);
        public Task<List<Order>> GetAllOrdersAsync();
        public Task<(bool Confirmed, string Response, Review? Review)> GetReviewFromOrderAsync(int id);
        public Task<(bool Confirmed, string Response)> CreateOrderAsync(CreateOrderDto request);
        public Task<(bool Confirmed, string Response)> UpdateOrderAsync(UpdateOrderDto request, int id);
        public Task<(bool Confirmed, string Response)> UpdateOrderServicesAsync(UpdateOrderDto request, int id);
        public Task<(bool Confirmed, string Response)> UpdateOrderStatusAsync(UpdateOrderDto request, int id);
        public Task<(bool Confirmed, string Response)> UpdateOrderPaidSellerAsync(int id);
        public Task<(bool Confirmed, string Response)> UpdateSentReview(UpdateReviewDto request, int id);
        public Task<(bool Confirmed, string Response)> DeleteOrderAsync(int id);
        public Task<(bool Confirmed, string Response)> DeleteSentReview(int id);
    }
}
