using EService.Dtos.ApplicationUserDtos;
using EService.Dtos.MessageDtos;
using EService.Models;

namespace EService.Services
{
    public interface IMessageService
    {
        public Task<ReturnMessageDto?> GetMessageAsync(int id);
        public Task<List<ReturnMessageDto>> GetAllMessagesAsync();
        public Task<(bool Confirmed, string Response)> CreateMessageAsync(CreateMessageDto request);
        public Task<(bool Confirmed, string Response)> UpdateMessageAsync(UpdateMessageDto request, int id);
        public Task<(bool Confirmed, string Response)> DeleteMessageAsync(int id);
    }
}
