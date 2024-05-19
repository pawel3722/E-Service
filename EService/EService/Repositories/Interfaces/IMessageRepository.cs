using EService.Models;

namespace EService.Repositories.Interfaces
{
    public interface IMessageRepository
    {
        public Task<Message?> GetMessageByIdAsync(int id);
        public Task<List<Message>> GetAllMessagesAsync();
        public Task<List<Message>> GetAllMessagesSentByToAsync(int senderId, int receiverId);
        public Task<List<Message>> GetAllMessagesSentByAsync(int senderId);
        public Task<List<Message>> GetAllMessagesSentToAsync(int receiverId);
        public Task AddMessageAsync(Message Message);
        public Task RemoveMessageAsync(Message Message);
        public Task SaveChangesAsync();
    }
}
