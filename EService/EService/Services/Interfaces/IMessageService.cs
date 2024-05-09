using EService.Dtos.MessageDtos;
using EService.Models;

namespace EService.Services
{
    public interface IMessageService
    {
        public Task<Message?> GetMessageAsync(int id);
        public Task<List<Message>> GetAllMessagesAsync();
        public Task<(bool Confirmed, string Response)> CreateMessageAsync(CreateMessageDto request);
        public Task<(bool Confirmed, string Response)> UpdateMessageAsync(UpdateMessageDto request, int id);
        public Task<(bool Confirmed, string Response)> DeleteMessageAsync(int id);
    }
}
