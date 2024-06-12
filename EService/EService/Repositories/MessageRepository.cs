using EService.Data;
using EService.Models;
using EService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EService.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly ApplicationDbContext _context;

        public MessageRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Message?> GetMessageByIdAsync(int id)
        {
            return await Task.Run(() => _context.Messages.Where(m => m.Id == id).
            Include(m => m.ReceivingUser).
            Include(m => m.SendingUser).            
            FirstOrDefaultAsync());
        }
        public async Task<List<Message>> GetAllMessagesAsync()
        {
            return await Task.Run(() => _context.Messages.
            Include(m => m.ReceivingUser).
            Include(m => m.SendingUser).
            ToListAsync());
        }
        public async Task<List<Message>> GetAllMessagesSentByToAsync(int senderId, int receiverId)
        {
            return await Task.Run(() => _context.Messages.
            Where(m => m.SendingUserId == senderId && m.ReceivingUserId == receiverId).
            Include(m => m.ReceivingUser).
            Include(m => m.SendingUser).
            ToListAsync());
        }
        public async Task<List<Message>> GetAllMessagesSentByAsync(int senderId)
        {
            return await Task.Run(() => _context.Messages.
            Where(m => m.SendingUserId == senderId).
            Include(m => m.ReceivingUser).
            Include(m => m.SendingUser).
            ToListAsync());
        }
        public async Task<List<Message>> GetAllMessagesSentToAsync(int receiverId)
        {
            return await Task.Run(() => _context.Messages.
            Where(m => m.ReceivingUserId == receiverId).
            Include(m => m.ReceivingUser).
            Include(m => m.SendingUser).
            ToListAsync());
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
