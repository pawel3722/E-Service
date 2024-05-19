using EService.Dtos.MessageDtos;
using EService.Models;

namespace EService.Services
{
    public interface IMessageService
    {
        public Task<Message?> GetMessageAsync(int id);
        public Task<(bool Confirmed, string Response, List<Message>? Messages)> GetSentMessagesAsync(int? receiverId);
        public Task<(bool Confirmed, string Response, List<Message>? Messages)> GetReceivedMessagesAsync(int? senderId);
        public Task<List<Message>> GetAllMessagesAsync();
        public Task<(bool Confirmed, string Response)> CreateMessageAsync(CreateMessageDto request);
        public Task<(bool Confirmed, string Response)> UpdateMessageAsync(UpdateMessageDto request, int id);
        public Task<(bool Confirmed, string Response)> UpdateSentMessageAsync(UpdateMessageDto request, int id);
        public Task<(bool Confirmed, string Response)> DeleteMessageAsync(int id);
        public Task<(bool Confirmed, string Response)> DeleteSentMessageAsync(int id);
    }
}
