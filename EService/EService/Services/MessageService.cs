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
        }
        public async Task<List<Message>> GetAllMessagesAsync()
        {
            return await _messageRepository.GetAllMessagesAsync();
        }
        public async Task<Message?> GetMessageAsync(int id)
        {
            return await _messageRepository.GetMessageByIdAsync(id);
        }
        public async Task<(bool Confirmed, string Response, List<Message>? Messages)> GetSentMessagesAsync(int? receiverId)
        {
            var sendingUser = await _authRepository.GetUserByIdAsync(Int32.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!));
            if (sendingUser != null)
            {
                ApplicationUser? receivingUser = null;
                if (receiverId != null)
                {
                    receivingUser = await _authRepository.GetUserByIdAsync(receiverId.Value);
                    if (receivingUser != null)
                    {
                        List<Message> messages = await _messageRepository.GetAllMessagesSentByToAsync(sendingUser.Id, receivingUser.Id);
                        return await Task.FromResult<(bool, string, List<Message>?)>((true, "", messages));
                    }
                    else return await Task.FromResult<(bool, string, List<Message>?)>((false, "Receiving user not found.", null));
                }
                else
                {
                    List<Message> messages = await _messageRepository.GetAllMessagesSentByAsync(sendingUser.Id);
                    return await Task.FromResult<(bool, string, List<Message>?)>((true, "", messages));
                }
            }
            return await Task.FromResult<(bool, string, List<Message>?)>((false, "Sending user not found.", null));
        }
        public async Task<(bool Confirmed, string Response, List<Message>? Messages)> GetReceivedMessagesAsync(int? senderId)
        {
            var receivingUser = await _authRepository.GetUserByIdAsync(Int32.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!));
            if (receivingUser != null)
            {
                ApplicationUser? sendingUser = null;
                if (senderId != null)
                {
                    sendingUser = await _authRepository.GetUserByIdAsync(senderId.Value);
                    if (sendingUser != null)
                    {
                        List<Message> messages = await _messageRepository.GetAllMessagesSentByToAsync(sendingUser.Id, receivingUser.Id);
                        return await Task.FromResult<(bool, string, List<Message>?)>((true, "", messages));
                    }
                    else return await Task.FromResult<(bool, string, List<Message>?)>((false, "Receiving user not found.", null));
                }
                else
                {
                    List<Message> messages = await _messageRepository.GetAllMessagesSentToAsync(receivingUser.Id);
                    return await Task.FromResult<(bool, string, List<Message>?)>((true, "", messages));
                }
            }
            return await Task.FromResult<(bool, string, List<Message>?)>((false, "Sending user not found.", null));
        }
        public async Task<(bool Confirmed, string Response)> CreateMessageAsync(CreateMessageDto request)
        {
            var sendingUser = await _authRepository.GetUserByIdAsync(Int32.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!));
            if(sendingUser != null)
            {
                var receivingUser = await _authRepository.GetUserByIdAsync(request.ReceivingUserId);
                if(receivingUser != null)
                {
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
                return await Task.FromResult((false, "Receiving user not found."));
            }
            return await Task.FromResult((false, "Sending user not found."));
        }
        public async Task<(bool Confirmed, string Response)> UpdateMessageAsync(UpdateMessageDto request, int id)
        {
            var message = await _messageRepository.GetMessageByIdAsync(id);
            if (message != null)
            {
                if (request.Text != null) message.Text = request.Text!;
                if (request.SendingDate != null) message.SendingDate = request.SendingDate.Value;
                await _messageRepository.SaveChangesAsync();
                return await Task.FromResult((true, "Message successfully updated."));
            }
            else return await Task.FromResult((false, "Message with given id does not exist."));
        }
        public async Task<(bool Confirmed, string Response)> UpdateSentMessageAsync(UpdateMessageDto request, int id)
        {
            var sendingUser = await _authRepository.GetUserByIdAsync(Int32.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!));
            if (sendingUser != null)
            {
                var message = await _messageRepository.GetMessageByIdAsync(id);
                if (message != null)
                {
                    if(message.SendingUserId == sendingUser.Id)
                    {
                        if (request.Text != null) message.Text = request.Text!;
                        if (request.SendingDate != null) message.SendingDate = request.SendingDate.Value;
                        await _messageRepository.SaveChangesAsync();
                        return await Task.FromResult((true, "Message successfully updated."));
                    }
                    else return await Task.FromResult((false, "Cannot update a message sent by a different user."));
                }
                else return await Task.FromResult((false, "Message with given id does not exist."));
            }
            else return await Task.FromResult((false, "Sending user with given id does not exist."));
        }

        public async Task<(bool Confirmed, string Response)> DeleteMessageAsync(int id)
        {
            var message = await _messageRepository.GetMessageByIdAsync(id);
            if (message != null)
            {
                await _messageRepository.RemoveMessageAsync(message);
                return await Task.FromResult((true, "Message successfully deleted."));
            }
            else return await Task.FromResult((false, "Message with given id does not exist."));
        }

        public async Task<(bool Confirmed, string Response)> DeleteSentMessageAsync(int id)
        {
            var sendingUser = await _authRepository.GetUserByIdAsync(Int32.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!));
            if (sendingUser != null)
            {
                var message = await _messageRepository.GetMessageByIdAsync(id);
                if (message != null)
                {
                    if (message.SendingUserId == sendingUser.Id)
                    {
                        await _messageRepository.RemoveMessageAsync(message);
                        return await Task.FromResult((true, "Message successfully deleted."));
                    }
                    else return await Task.FromResult((false, "Cannot delete a message sent by a different user."));
                }
                else return await Task.FromResult((false, "Message with given id does not exist."));
            }
            else return await Task.FromResult((false, "Sending user with given id does not exist."));
        }
    }
}
