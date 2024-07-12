using AutoMapper;
using EService.Dtos.ApplicationUserDtos;
using EService.Dtos.MessageDtos;
using EService.Dtos.OrderDtos;
using EService.Dtos.ReviewDtos;
using EService.Dtos.RolesDtos;
using EService.Dtos.ServiceDtos;
using EService.Models;
using EService.Repositories;
using EService.Repositories.Interfaces;
using EService.Services.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Security.Claims;

namespace EService.Services
{
    public class ApplicationUserService : IApplicationUserService
    {
        private readonly IApplicationUserRepository _applicationUserRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public ApplicationUserService(IApplicationUserRepository applicationUserRepository, IMessageRepository messageRepository, IOrderRepository orderRepository, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _applicationUserRepository = applicationUserRepository;
            _messageRepository = messageRepository;
            _orderRepository = orderRepository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<List<ReturnApplicationUserDto>> GetAllUsersAsync()
        {
            var users = await _applicationUserRepository.GetAllUsersAsync();
            return _mapper.Map<List<ReturnApplicationUserDto>>(users);
        }
        public async Task<ReturnApplicationUserDto?> GetUserAsync(int id)
        {
            var user = await _applicationUserRepository.GetUserByIdAsync(id);
            return _mapper.Map<ReturnApplicationUserDto>(user);
        }
        public async Task<List<ReturnRoleDto>> GetAllRolesAsync()
        {
            var roles = await _applicationUserRepository.GetAllRolesAsync();
            return _mapper.Map<List<ReturnRoleDto>>(roles);
        }
        public async Task<ReturnRoleDto?> GetRoleAsync(int id)
        {
            var role = await _applicationUserRepository.GetRoleByIdAsync(id);
            return _mapper.Map<ReturnRoleDto>(role);
        }
        public async Task<(bool Confirmed, string Response)> AddUserRolesAsync(UpdateRolesDto request, int id)
        {
            var user = await _applicationUserRepository.GetUserByIdAsync(id);
            if (user == null) return await Task.FromResult((false, "User with given id does not exist."));
            if (request.RoleNames.Count == 0) return await Task.FromResult((false, "No fields to be updated."));
            List<Role> roles = new List<Role>();
            foreach (var roleName in request.RoleNames)
            {
                var role = await _applicationUserRepository.GetRoleByNameAsync(roleName);
                if (role == null) return await Task.FromResult((false, "Role with given name does not exist."));
                if (role == null || role.Name == roleName) return await Task.FromResult((false, $"User already has the role {roleName}."));
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
            if (request.RoleNames.Count == 0) return await Task.FromResult((false, "No fields to be updated."));
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
        public async Task<(bool Confirmed, string Response, List<ReturnMessageDto>? Messages)> GetSentMessagesAsync(int? receiverId)
        {
            var sendingUser = await _applicationUserRepository.GetUserByIdAsync(Int32.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!));
            if (sendingUser == null) return await Task.FromResult<(bool, string, List<ReturnMessageDto>?)>((false, "Sending user not found.", null));
            ApplicationUser? receivingUser = null;
            if (receiverId != null)
            {
                receivingUser = await _applicationUserRepository.GetUserByIdAsync(receiverId.Value);
                if (receivingUser == null) return await Task.FromResult<(bool, string, List<ReturnMessageDto>?)>((false, "Receiving user not found.", null));
                List<Message> messages = await _messageRepository.GetAllMessagesSentByToAsync(sendingUser.Id, receivingUser.Id);
                var messagesDto = _mapper.Map<List<ReturnMessageDto>>(messages);
                return await Task.FromResult<(bool, string, List<ReturnMessageDto>?)>((true, "", messagesDto));
            }
            else
            {
                List<Message> messages = await _messageRepository.GetAllMessagesSentByAsync(sendingUser.Id);
                var messagesDto = _mapper.Map<List<ReturnMessageDto>>(messages);
                return await Task.FromResult<(bool, string, List<ReturnMessageDto>?)>((true, "", messagesDto));
            }
        }
        public async Task<(bool Confirmed, string Response, List<ReturnMessageDto>? Messages)> GetReceivedMessagesAsync(int? senderId)
        {
            var receivingUser = await _applicationUserRepository.GetUserByIdAsync(Int32.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!));
            if (receivingUser == null) return await Task.FromResult<(bool, string, List<ReturnMessageDto>?)>((false, "Sending user not found.", null));
            ApplicationUser? sendingUser = null;
            if (senderId != null)
            {
                sendingUser = await _applicationUserRepository.GetUserByIdAsync(senderId.Value);
                if (sendingUser == null) return await Task.FromResult<(bool, string, List<ReturnMessageDto>?)>((false, "Receiving user not found.", null));
                List<Message> messages = await _messageRepository.GetAllMessagesSentByToAsync(sendingUser.Id, receivingUser.Id);
                var messagesDto = _mapper.Map<List<ReturnMessageDto>>(messages);
                return await Task.FromResult<(bool, string, List<ReturnMessageDto>?)>((true, "", messagesDto));
            }
            else
            {
                List<Message> messages = await _messageRepository.GetAllMessagesSentToAsync(receivingUser.Id);
                var messagesDto = _mapper.Map<List<ReturnMessageDto>>(messages);
                return await Task.FromResult<(bool, string, List<ReturnMessageDto>?)>((true, "", messagesDto));
            }
        }
        public async Task<(bool Confirmed, string Response)> UpdateSentMessageAsync(UpdateMessageDto request, int id)
        {
            var sendingUser = await _applicationUserRepository.GetUserByIdAsync(Int32.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!));
            if (sendingUser == null) return await Task.FromResult((false, "Sending user with given id does not exist."));
            var message = await _messageRepository.GetMessageByIdAsync(id);
            if (message == null) return await Task.FromResult((false, "Message with given id does not exist."));
            if (message.SendingUserId != sendingUser.Id) return await Task.FromResult((false, "Cannot update a message sent by a different user."));

            if (!(request.Text != null && request.Text != message.Text 
                || request.SendingDate != null && request.SendingDate != message.SendingDate))
                return await Task.FromResult((false, "No fields to be updated."));
            if(request.Text != null) message.Text = request.Text!;
            if(request.SendingDate != null) message.SendingDate = request.SendingDate.Value;
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
        public async Task<(bool Confirmed, string Response, List<ReturnOrderDto>? Orders)> GetCustomerOrdersAsync()
        {
            var user = await _applicationUserRepository.GetUserByIdAsync(Int32.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!));
            if (user == null) return await Task.FromResult<(bool, string, List<ReturnOrderDto>?)>((false, "User not found.", null));
            var orders = await _orderRepository.GetCustomerOrdersAsync(user.Id);
            var ordersDto = _mapper.Map<List<ReturnOrderDto>>(orders);
            return await Task.FromResult((true, "", ordersDto));
        }
        public async Task<(bool Confirmed, string Response, List<ReturnOrderDto>? Orders)> GetManagerOrdersAsync()
        {
            var user = await _applicationUserRepository.GetUserByIdAsync(Int32.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!));
            if (user == null) return await Task.FromResult<(bool, string, List<ReturnOrderDto>?)>((false, "User not found.", null));
            var orders = await _orderRepository.GetManagerOrdersAsync(user.Id);
            var ordersDto = _mapper.Map<List<ReturnOrderDto>>(orders);
            return await Task.FromResult((true, "", ordersDto));
        }
        public async Task<(bool Confirmed, string Response, List<ReturnServiceDto>? Services)> GetClientServices(int? orderId)
        {
            var user = await _applicationUserRepository.GetUserByIdAsync(Int32.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!));

            List<ReturnServiceDto> servicesDto;
            if (user == null) return await Task.FromResult<(bool, string, List<ReturnServiceDto>?)>((false, "User not found.", null));
            if (orderId == null)
            {
                var services = new List<Service>();
                foreach (var customerOrder in user.CustomerOrders)
                {
                    services.AddRange(customerOrder.Services);
                }
                servicesDto = _mapper.Map<List<ReturnServiceDto>>(services);
                return await Task.FromResult((true, "", servicesDto));
            }
            var order = await _orderRepository.GetOrderByIdAsync(orderId.Value);
            if (order == null) return await Task.FromResult<(bool, string, List<ReturnServiceDto>?)>((false, "Order with given id does not exist.", null));
            if (order.CustomerId != user.Id) return await Task.FromResult<(bool, string, List<ReturnServiceDto>?)>((false, "Order with given id was not made by the user.", null));
            servicesDto = _mapper.Map<List<ReturnServiceDto>>(order.Services);
            return await Task.FromResult((true, "", servicesDto));
        }
        public async Task<(bool Confirmed, string Response, List<ReturnServiceDto>? Services)> GetServicemanServices(int? orderId)
        {
            var user = await _applicationUserRepository.GetUserByIdAsync(Int32.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!));
            List<ReturnServiceDto> servicesDto;
            if (user == null) return await Task.FromResult<(bool, string, List<ReturnServiceDto>?)>((false, "User not found.", null));
            servicesDto = _mapper.Map<List<ReturnServiceDto>>(user.Services);
            if (orderId == null) return await Task.FromResult((true, "", servicesDto));
            var order = await _orderRepository.GetOrderByIdAsync(orderId.Value);
            if (order == null) return await Task.FromResult<(bool, string, List<ReturnServiceDto>?)>((false, "Order with given id does not exist.", null));
            if (order.CustomerId != user.Id) return await Task.FromResult<(bool, string, List<ReturnServiceDto>?)>((false, "Order with given id was not made by the user.", null));
            servicesDto = _mapper.Map<List<ReturnServiceDto>>(order.Services);
            return await Task.FromResult((true, "", servicesDto));
        }
    }
}
