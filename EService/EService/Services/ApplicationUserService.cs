using EService.Dtos.MessageDtos;
using EService.Dtos.RolesDtos;
using EService.Dtos.ServiceDtos;
using EService.Models;
using EService.Repositories;
using EService.Repositories.Interfaces;
using EService.Services.Interfaces;
using System.Security.Claims;

namespace EService.Services
{
    public class ApplicationUserService : IApplicationUserService
    {
        private readonly IApplicationUserRepository _applicationUserRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApplicationUserService(IApplicationUserRepository applicationUserRepository, IMessageRepository messageRepository, IOrderRepository orderRepository, IHttpContextAccessor httpContextAccessor)
        {
            _applicationUserRepository = applicationUserRepository;
            _messageRepository = messageRepository;
            _orderRepository = orderRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<ApplicationUser>> GetAllUsersAsync()
        {
            return await _applicationUserRepository.GetAllUsersAsync();
        }
        public async Task<ApplicationUser?> GetUserAsync(int id)
        {
            return await _applicationUserRepository.GetUserByIdAsync(id);
        }
        public async Task<List<Role>> GetAllRolesAsync()
        {
            return await _applicationUserRepository.GetAllRolesAsync();
        }
        public async Task<Role?> GetRoleAsync(int id)
        {
            return await _applicationUserRepository.GetRoleByIdAsync(id);
        }
        public async Task<(bool Confirmed, string Response)> AddUserRolesAsync(UpdateRolesDto request, int id)
        {
            var user = await _applicationUserRepository.GetUserByIdAsync(id);
            if (user == null) return await Task.FromResult((false, "User with given id does not exist."));
            List<Role> roles = new List<Role>();
            foreach (var roleName in request.RoleNames)
            {
                var role = await _applicationUserRepository.GetRoleByNameAsync(roleName);
                if (role == null) return await Task.FromResult((false, "Role with given name does not exist."));
                user.Roles.Add(role);
                role.Users.Add(user);
            }
            await _applicationUserRepository.SaveChangesAsync();
            return await Task.FromResult((true, "User's roles successfully added."));
        }
        public async Task<(bool Confirmed, string Response)> RemoveUserRolesAsync(UpdateRolesDto request, int id)
        {
            var user = await _applicationUserRepository.GetUserByIdAsync(id);
            if (user == null) return await Task.FromResult((false, "User with given id does not exist."));
            List<Role> roles = new List<Role>();
            foreach (var roleName in request.RoleNames)
            {
                var role = await _applicationUserRepository.GetRoleByNameAsync(roleName);
                if (role == null) return await Task.FromResult((false, "Role with given name does not exist."));
                if (user.Roles.Contains(role)) user.Roles.Remove(role);
                else return await Task.FromResult((false, "Role is not performed by this user."));
                if (role.Users.Contains(user)) role.Users.Remove(user);
                else return await Task.FromResult((false, "User does not perform this role."));
            }
            await _applicationUserRepository.SaveChangesAsync();
            return await Task.FromResult((true, "User's roles successfully removed."));
        }
        public async Task<(bool Confirmed, string Response)> DeleteUserAsync(int id)
        {
            var user = await _applicationUserRepository.GetUserByIdAsync(id);
            if (user == null) return await Task.FromResult((false, "User with given id does not exist."));
            await _applicationUserRepository.RemoveUserAsync(user);
            return await Task.FromResult((true, "User successfully deleted."));
        }
        public async Task<(bool Confirmed, string Response, List<Message>? Messages)> GetSentMessagesAsync(int? receiverId)
        {
            var sendingUser = await _applicationUserRepository.GetUserByIdAsync(Int32.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!));
            if (sendingUser == null) return await Task.FromResult<(bool, string, List<Message>?)>((false, "Sending user not found.", null));
            ApplicationUser? receivingUser = null;
            if (receiverId != null)
            {
                receivingUser = await _applicationUserRepository.GetUserByIdAsync(receiverId.Value);
                if (receivingUser == null) return await Task.FromResult<(bool, string, List<Message>?)>((false, "Receiving user not found.", null));
                List<Message> messages = await _messageRepository.GetAllMessagesSentByToAsync(sendingUser.Id, receivingUser.Id);
                return await Task.FromResult<(bool, string, List<Message>?)>((true, "", messages));
            }
            else
            {
                List<Message> messages = await _messageRepository.GetAllMessagesSentByAsync(sendingUser.Id);
                return await Task.FromResult<(bool, string, List<Message>?)>((true, "", messages));
            }
        }
        public async Task<(bool Confirmed, string Response, List<Message>? Messages)> GetReceivedMessagesAsync(int? senderId)
        {
            var receivingUser = await _applicationUserRepository.GetUserByIdAsync(Int32.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!));
            if (receivingUser == null) return await Task.FromResult<(bool, string, List<Message>?)>((false, "Sending user not found.", null));
            ApplicationUser? sendingUser = null;
            if (senderId != null)
            {
                sendingUser = await _applicationUserRepository.GetUserByIdAsync(senderId.Value);
                if (sendingUser == null) return await Task.FromResult<(bool, string, List<Message>?)>((false, "Receiving user not found.", null));
                List<Message> messages = await _messageRepository.GetAllMessagesSentByToAsync(sendingUser.Id, receivingUser.Id);
                return await Task.FromResult<(bool, string, List<Message>?)>((true, "", messages));
            }
            else
            {
                List<Message> messages = await _messageRepository.GetAllMessagesSentToAsync(receivingUser.Id);
                return await Task.FromResult<(bool, string, List<Message>?)>((true, "", messages));
            }
        }
        public async Task<(bool Confirmed, string Response)> UpdateSentMessageAsync(UpdateMessageDto request, int id)
        {
            var sendingUser = await _applicationUserRepository.GetUserByIdAsync(Int32.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!));
            if (sendingUser == null) return await Task.FromResult((false, "Sending user with given id does not exist."));
            var message = await _messageRepository.GetMessageByIdAsync(id);
            if (message == null) return await Task.FromResult((false, "Message with given id does not exist."));
            if (message.SendingUserId != sendingUser.Id) return await Task.FromResult((false, "Cannot update a message sent by a different user."));
            if (request.Text != null) message.Text = request.Text!;
            if (request.SendingDate != null) message.SendingDate = request.SendingDate.Value;
            await _messageRepository.SaveChangesAsync();
            return await Task.FromResult((true, "Message successfully updated."));
        }
        public async Task<(bool Confirmed, string Response)> DeleteSentMessageAsync(int id)
        {
            var sendingUser = await _applicationUserRepository.GetUserByIdAsync(Int32.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!));
            if (sendingUser == null) return await Task.FromResult((false, "Sending user with given id does not exist."));
            var message = await _messageRepository.GetMessageByIdAsync(id);
            if (message == null) return await Task.FromResult((false, "Message with given id does not exist."));
            if (message.SendingUserId != sendingUser.Id) return await Task.FromResult((false, "Cannot delete a message sent by a different user."));
            await _messageRepository.RemoveMessageAsync(message);
            return await Task.FromResult((true, "Message successfully deleted."));
        }
        public async Task<(bool Confirmed, string Response, List<Order>? Orders)> GetCustomerOrdersAsync()
        {
            var user = await _applicationUserRepository.GetUserByIdAsync(Int32.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!));
            if (user == null) return await Task.FromResult<(bool, string, List<Order>?)>((false, "User not found.", null));
            return await Task.FromResult((true, "", await _orderRepository.GetCustomerOrdersAsync(user.Id)));
        }
        public async Task<(bool Confirmed, string Response, List<Order>? Orders)> GetManagerOrdersAsync()
        {
            var user = await _applicationUserRepository.GetUserByIdAsync(Int32.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!));
            if (user == null) return await Task.FromResult<(bool, string, List<Order>?)>((false, "User not found.", null));
            return await Task.FromResult((true, "", await _orderRepository.GetManagerOrdersAsync(user.Id)));
        }
        public async Task<(bool Confirmed, string Response, List<Service>? Services)> GetClientServices(int? orderId)
        {
            var user = await _applicationUserRepository.GetUserByIdAsync(Int32.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!));
            if (user == null) return await Task.FromResult<(bool, string, List<Service>?)>((false, "User not found.", null));
            if (orderId == null)
            {
                var services = new List<Service>();
                foreach (var customerOrder in user.CustomerOrders)
                {
                    services.AddRange(customerOrder.Services);
                }
                return await Task.FromResult((true, "", services));
            }
            var order = await _orderRepository.GetOrderByIdAsync(orderId.Value);
            if (order == null) return await Task.FromResult<(bool, string, List<Service>?)>((false, "Order with given id does not exist.", null));
            if (order.CustomerId != user.Id) return await Task.FromResult<(bool, string, List<Service>?)>((false, "Order with given id was not made by the user.", null));
            return await Task.FromResult((true, "", order.Services));
        }
        public async Task<(bool Confirmed, string Response, List<Service>? Services)> GetServicemanServices(int? orderId)
        {
            var user = await _applicationUserRepository.GetUserByIdAsync(Int32.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!));
            if (user == null) return await Task.FromResult<(bool, string, List<Service>?)>((false, "User not found.", null));
            if (orderId == null) return await Task.FromResult((true, "", user.Services));
            var order = await _orderRepository.GetOrderByIdAsync(orderId.Value);
            if (order == null) return await Task.FromResult<(bool, string, List<Service>?)>((false, "Order with given id does not exist.", null));
            if (order.CustomerId != user.Id) return await Task.FromResult<(bool, string, List<Service>?)>((false, "Order with given id was not made by the user.", null));
            return await Task.FromResult((true, "", order.Services));
        }
        public async Task<(bool Confirmed, string Response, Review? Review)> GetReviewFromOrderAsync(int id)
        {
            var user = await _applicationUserRepository.GetUserByIdAsync(Int32.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!));
            if (user == null) return await Task.FromResult<(bool Confirmed, string Response, Review? Review)>((false, "User with given id does not exist.", null));
            var order = await _orderRepository.GetOrderByIdAsync(id);
            if (order == null) return await Task.FromResult<(bool Confirmed, string Response, Review? Review)>((false, "Order with given id does not exist.", null));
            if (user.Id != order.CustomerId) return await Task.FromResult<(bool Confirmed, string Response, Review? Review)>((false, "Order with given id does not belong to this user.", null));
            return await Task.FromResult((true, "", order.Review));
        }
    }
}
