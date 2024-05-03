using EService.Dtos.MessageDtos;
using EService.Models;

namespace EService.Services
{
    public interface IMessageService
    {
        public Task<Message?> GetMessage(int id);
        public Task<List<Message>> GetAllMessages();
        public Task<(bool Confirmed, string Response)> CreateMessage(MessageDto request);
        public Task<(bool Confirmed, string Response)> UpdateMessage(MessageDto request, int id);
        public Task<(bool Confirmed, string Response)> DeleteMessage(int id);
    }
}
