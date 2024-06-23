using EService.Dtos.ApplicationUserDtos;
using EService.Dtos.MessageDtos;
using EService.Dtos.OrderDtos;
using EService.Dtos.ReviewDtos;
using EService.Dtos.RolesDtos;
using EService.Dtos.ServiceDtos;
using EService.Models;

namespace EService.Services.Interfaces
{
    public interface IApplicationUserService
    {
        public Task<List<ReturnApplicationUserDto>> GetAllUsersAsync();
        public Task<ReturnApplicationUserDto?> GetUserAsync(int id);
        public Task<List<ReturnRoleDto>> GetAllRolesAsync();
        public Task<ReturnRoleDto?> GetRoleAsync(int id);
        public Task<(bool Confirmed, string Response)> AddUserRolesAsync(UpdateRolesDto request, int id);
        public Task<(bool Confirmed, string Response)> RemoveUserRolesAsync(UpdateRolesDto request, int id);
        public Task<(bool Confirmed, string Response)> DeleteUserAsync(int id);
        public Task<(bool Confirmed, string Response, List<ReturnMessageDto>? Messages)> GetSentMessagesAsync(int? receiverId);
        public Task<(bool Confirmed, string Response, List<ReturnMessageDto>? Messages)> GetReceivedMessagesAsync(int? senderId);
        public Task<(bool Confirmed, string Response)> UpdateSentMessageAsync(UpdateMessageDto request, int id);
        public Task<(bool Confirmed, string Response)> DeleteSentMessageAsync(int id);
        public Task<(bool Confirmed, string Response, List<ReturnOrderDto>? Orders)> GetCustomerOrdersAsync();
        public Task<(bool Confirmed, string Response, List<ReturnOrderDto>? Orders)> GetManagerOrdersAsync();
        public Task<(bool Confirmed, string Response, List<ReturnServiceDto>? Services)> GetClientServices(int? orderId);
        public Task<(bool Confirmed, string Response, List<ReturnServiceDto>? Services)> GetServicemanServices(int? orderId);
        public Task<(bool Confirmed, string Response, ReturnReviewDto? Review)> GetReviewFromOrderAsync(int id);
    }
}
