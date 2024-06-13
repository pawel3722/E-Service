using AutoMapper;
using Azure.Core;
using EService.Dtos.ApplicationUserDtos;
using EService.Dtos.MessageDtos;
using EService.Models;
using EService.Repositories.Interfaces;
using System.Security.Claims;
using static System.Net.Mime.MediaTypeNames;

namespace EService.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IApplicationUserRepository _applicationUserRepository;
        private readonly IMapper _mapper;

        public MessageService(IMessageRepository messageRepository, IHttpContextAccessor contextAccessor, IApplicationUserRepository applicationUserRepository, IMapper mapper)
        {
            _messageRepository = messageRepository;
            _httpContextAccessor = contextAccessor;
            _applicationUserRepository = applicationUserRepository;
            _mapper = mapper;
        }
        public async Task<List<ReturnApplicationUserDto>> GetAllMessagesAsync()
        {
            var messages = await _messageRepository.GetAllMessagesAsync();
            return _mapper.Map<List<ReturnApplicationUserDto>>(messages);
           // return await _messageRepository.GetAllMessagesAsync();
        }
        public async Task<ReturnApplicationUserDto?> GetMessageAsync(int id)
        {
            var message = await _messageRepository.GetMessageByIdAsync(id);
            return _mapper.Map<ReturnApplicationUserDto>(message);
            //return await _messageRepository.GetMessageByIdAsync(id);
        }
        public async Task<(bool Confirmed, string Response)> CreateMessageAsync(CreateMessageDto request)
        {
            var sendingUser = await _applicationUserRepository.GetUserByIdAsync(Int32.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!));
            if(sendingUser == null) return await Task.FromResult((false, "Sending user not found."));
            var receivingUser = await _applicationUserRepository.GetUserByIdAsync(request.ReceivingUserId);
            if(receivingUser == null) return await Task.FromResult((false, "Receiving user not found."));
            var message = new Message
            {
                Text = request.Text,
                SendingDate = request.SendingDate,
                SendingUserId = sendingUser.Id,
                SendingUser = sendingUser,
                ReceivingUserId = request.ReceivingUserId,
                ReceivingUser = receivingUser
            };
            sendingUser.SentMessages.Add(message);
            receivingUser.ReceivedMessages.Add(message);
            await _messageRepository.AddMessageAsync(message);
            return await Task.FromResult((true, "Message successfully created."));  
        }
        public async Task<(bool Confirmed, string Response)> UpdateMessageAsync(UpdateMessageDto request, int id)
        {
            var message = await _messageRepository.GetMessageByIdAsync(id);
            if(message == null) return await Task.FromResult((false, "Message with given id does not exist."));
            if(request.Text != null) message.Text = request.Text!;
            if(request.SendingDate != null) message.SendingDate = request.SendingDate.Value;
            await _messageRepository.SaveChangesAsync();
            return await Task.FromResult((true, "Message successfully updated."));
        }
        public async Task<(bool Confirmed, string Response)> DeleteMessageAsync(int id)
        {
            var message = await _messageRepository.GetMessageByIdAsync(id);
            if (message == null) return await Task.FromResult((false, "Message with given id does not exist."));
            await _messageRepository.RemoveMessageAsync(message);
            return await Task.FromResult((true, "Message successfully deleted."));
        }
    }
}
