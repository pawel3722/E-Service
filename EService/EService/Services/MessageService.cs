using EService.Dtos.MessageDtos;
using EService.Models;
using EService.Repositories;
using static System.Net.Mime.MediaTypeNames;

namespace EService.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        public MessageService(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }
        public async Task<List<Message>> GetAllMessages()
        {
            return await _messageRepository.GetAllMessages();
        }
        public async Task<Message?> GetMessage(int id)
        {
            return await _messageRepository.GetMessageById(id);
        }
        public async Task<(bool Confirmed, string Response)> CreateMessage(MessageDto request)
        {
            var message = new Message
            {
                Text = request.Text,
                SendingDate = request.SendingDate,
                ReceivingDate = request.ReceivingDate,
                SendingUserId = request.SendingUserId,
                ReceivingUserId = request.ReceivingUserId
            };
            await _messageRepository.AddMessageAsync(message);
            return await Task.FromResult((true, "Message successfully created."));
        }
        public async Task<(bool Confirmed, string Response)> UpdateMessage(MessageDto request, int id)
        {
            var message = await _messageRepository.GetMessageById(id);
            if (message != null)
            {
                message.Text = request.Text;
                message.SendingDate = request.SendingDate;
                message.ReceivingDate = request.ReceivingDate;
                message.SendingUserId = request.SendingUserId;
                message.ReceivingUserId = request.ReceivingUserId;
                await _messageRepository.SaveChangesAsync();
                return await Task.FromResult((true, "Message successfully updated."));
            }
            else return await Task.FromResult((false, "Message with given id does not exist."));
        }

        public async Task<(bool Confirmed, string Response)> DeleteMessage(int id)
        {
            var message = await _messageRepository.GetMessageById(id);
            if (message != null)
            {
                await _messageRepository.RemoveMessageAsync(message);
                return await Task.FromResult((true, "Message successfully deleted."));
            }
            else return await Task.FromResult((false, "Message with given id does not exist."));
        }
    }
}
