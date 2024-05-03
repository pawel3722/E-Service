using EService.Data;
using EService.Models;

namespace EService.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly ApplicationDbContext _context;

        public MessageRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Message?> GetMessageById(int id)
        {
            return await Task.Run(() => _context.Messages.FindAsync(id).Result);
        }
        public async Task<List<Message>> GetAllMessages()
        {
            return await Task.Run(() => _context.Messages.ToList());
        }
        public async Task AddMessageAsync(Message message)
        {
            await _context.AddAsync(message);
            await SaveChangesAsync();
        }
        public async Task RemoveMessageAsync(Message message)
        {
            _context.Remove(message);
            await SaveChangesAsync();
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
