using Azure.Core;
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
        private readonly IAuthRepository _authRepository;
        public MessageService(IMessageRepository messageRepository, IHttpContextAccessor contextAccessor, IAuthRepository authRepository)
        {
            _messageRepository = messageRepository;
            _httpContextAccessor = contextAccessor;
            _authRepository = authRepository;
            //mapper
        }
        public async Task<List<Message>> GetAllMessagesAsync()
        {
            var messages = await _messageRepository.GetAllMessagesAsync();
            //mapowanie do dtosów
            //return dtosy, zamiast poniżej
            return await _messageRepository.GetAllMessagesAsync();
        }
        public async Task<Message?> GetMessageAsync(int id)
        {
            return await _messageRepository.GetMessageByIdAsync(id);
        }
        public async Task<(bool Confirmed, string Response, List<Message>? Messages)> GetSentMessagesAsync(int? receiverId)
        {
            var sendingUser = await _authRepository.GetUserByIdAsync(Int32.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!));
            if(sendingUser == null) return await Task.FromResult<(bool, string, List<Message>?)>((false, "Sending user not found.", null));
            ApplicationUser? receivingUser = null;
            if(receiverId != null)
            {
                receivingUser = await _authRepository.GetUserByIdAsync(receiverId.Value);
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
            var receivingUser = await _authRepository.GetUserByIdAsync(Int32.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!));
            if(receivingUser == null) return await Task.FromResult<(bool, string, List<Message>?)>((false, "Sending user not found.", null));
            ApplicationUser? sendingUser = null;
            if(senderId != null)
            {
                sendingUser = await _authRepository.GetUserByIdAsync(senderId.Value);
                if (sendingUser != null) return await Task.FromResult<(bool, string, List<Message>?)>((false, "Receiving user not found.", null));
                List<Message> messages = await _messageRepository.GetAllMessagesSentByToAsync(sendingUser.Id, receivingUser.Id);
                return await Task.FromResult<(bool, string, List<Message>?)>((true, "", messages));
            }
            else
            {
                List<Message> messages = await _messageRepository.GetAllMessagesSentToAsync(receivingUser.Id);
                return await Task.FromResult<(bool, string, List<Message>?)>((true, "", messages));
            }
        }
        public async Task<(bool Confirmed, string Response)> CreateMessageAsync(CreateMessageDto request)
        {
            var sendingUser = await _authRepository.GetUserByIdAsync(Int32.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!));
            if(sendingUser == null) return await Task.FromResult((false, "Sending user not found."));
            var receivingUser = await _authRepository.GetUserByIdAsync(request.ReceivingUserId);
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
        public async Task<(bool Confirmed, string Response)> UpdateSentMessageAsync(UpdateMessageDto request, int id)
        {
            var sendingUser = await _authRepository.GetUserByIdAsync(Int32.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!));
            if(sendingUser == null) return await Task.FromResult((false, "Sending user with given id does not exist."));
            var message = await _messageRepository.GetMessageByIdAsync(id);
            if(message == null) return await Task.FromResult((false, "Message with given id does not exist."));
            if(message.SendingUserId != sendingUser.Id) return await Task.FromResult((false, "Cannot update a message sent by a different user."));
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

        public async Task<(bool Confirmed, string Response)> DeleteSentMessageAsync(int id)
        {
            var sendingUser = await _authRepository.GetUserByIdAsync(Int32.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!));
            if (sendingUser == null) return await Task.FromResult((false, "Sending user with given id does not exist."));
            var message = await _messageRepository.GetMessageByIdAsync(id);
            if (message == null) return await Task.FromResult((false, "Message with given id does not exist."));
            if (message.SendingUserId != sendingUser.Id) return await Task.FromResult((false, "Cannot delete a message sent by a different user."));
            await _messageRepository.RemoveMessageAsync(message);
            return await Task.FromResult((true, "Message successfully deleted."));
        }
    }
}
