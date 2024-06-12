using EService.Dtos.MessageDtos;
using EService.Dtos.RolesDtos;
using EService.Dtos.ServiceDtos;
using EService.Models;

namespace EService.Services.Interfaces
{
    public interface IApplicationUserService
    {
        public Task<List<ApplicationUser>> GetAllUsersAsync();
        public Task<ApplicationUser?> GetUserAsync(int id);
        public Task<List<Role>> GetAllRolesAsync();
        public Task<Role?> GetRoleAsync(int id);
        public Task<(bool Confirmed, string Response)> AddUserRolesAsync(UpdateRolesDto request, int id);
        public Task<(bool Confirmed, string Response)> RemoveUserRolesAsync(UpdateRolesDto request, int id);
        public Task<(bool Confirmed, string Response)> DeleteUserAsync(int id);
        public Task<(bool Confirmed, string Response, List<Message>? Messages)> GetSentMessagesAsync(int? receiverId);
        public Task<(bool Confirmed, string Response, List<Message>? Messages)> GetReceivedMessagesAsync(int? senderId);
        public Task<(bool Confirmed, string Response)> UpdateSentMessageAsync(UpdateMessageDto request, int id);
        public Task<(bool Confirmed, string Response)> DeleteSentMessageAsync(int id);
        public Task<(bool Confirmed, string Response, List<Order>? Orders)> GetCustomerOrdersAsync();
        public Task<(bool Confirmed, string Response, List<Order>? Orders)> GetManagerOrdersAsync();
        public Task<(bool Confirmed, string Response, List<Service>? Services)> GetClientServices(int? orderId);
        public Task<(bool Confirmed, string Response, List<Service>? Services)> GetServicemanServices(int? orderId);
        public Task<(bool Confirmed, string Response, Review? Review)> GetReviewFromOrderAsync(int id);
    }
}
