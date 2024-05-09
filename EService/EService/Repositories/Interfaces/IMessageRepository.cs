using EService.Models;

namespace EService.Repositories.Interfaces
{
    public interface IMessageRepository
    {
        public Task<Message?> GetMessageByIdAsync(int id);
        public Task<List<Message>> GetAllMessagesAsync();
        public Task AddMessageAsync(Message Message);
        public Task RemoveMessageAsync(Message Message);
        public Task SaveChangesAsync();
    }
}
