using EService.Models;

namespace EService.Repositories
{
    public interface IMessageRepository
    {
        public Task<Message?> GetMessageById(int id);
        public Task<List<Message>> GetAllMessages();
        public Task AddMessageAsync(Message Message);
        public Task RemoveMessageAsync(Message Message);
        public Task SaveChangesAsync();
    }
}
