using EService.Data;
using EService.Models;
using EService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

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
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                                    new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                                    TransactionScopeAsyncFlowOption.Enabled);
            Message? msg = null;
            try
            {
                msg = await Task.Run(() => _context.Messages.Where(m => m.Id == id).
                    Include(m => m.ReceivingUser).
                    Include(m => m.SendingUser).
                    FirstOrDefaultAsync());
                scope.Complete();
            }
            catch (Exception) { }
            return await Task.Run(() => msg);
        }
        public async Task<List<Message>> GetAllMessagesAsync()
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                                    new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                                    TransactionScopeAsyncFlowOption.Enabled);
            List<Message> arr = new List<Message>();
            try
            {
                arr = await Task.Run(() => _context.Messages.
                    Include(m => m.ReceivingUser).
                    Include(m => m.SendingUser).
                    ToListAsync());
                scope.Complete();
            }
            catch (Exception) { }
            return await Task.Run(() => arr);
        }
        public async Task<List<Message>> GetAllMessagesSentByToAsync(int senderId, int receiverId)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                                    new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                                    TransactionScopeAsyncFlowOption.Enabled);
            List<Message> arr = new List<Message>();
            try
            {
                arr = await Task.Run(() => _context.Messages.
                    Where(m => m.SendingUserId == senderId && m.ReceivingUserId == receiverId).
                    Include(m => m.ReceivingUser).
                    Include(m => m.SendingUser).
                    ToListAsync());
                scope.Complete();
            }
            catch (Exception) { }
            return await Task.Run(() => arr);
        }
        public async Task<List<Message>> GetAllMessagesSentByAsync(int senderId)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                                    new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                                    TransactionScopeAsyncFlowOption.Enabled);
            List<Message> arr = new List<Message>();
            try
            {
                arr = await Task.Run(() => _context.Messages.
                    Where(m => m.SendingUserId == senderId).
                    Include(m => m.ReceivingUser).
                    Include(m => m.SendingUser).
                    ToListAsync());
                scope.Complete();
            }
            catch (Exception) { }
            return await Task.Run(() => arr);
        }
        public async Task<List<Message>> GetAllMessagesSentToAsync(int receiverId)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                                    new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                                    TransactionScopeAsyncFlowOption.Enabled);
            List<Message> arr = new List<Message>();
            try
            {
                arr = await Task.Run(() => _context.Messages.
                    Where(m => m.ReceivingUserId == receiverId).
                    Include(m => m.ReceivingUser).
                    Include(m => m.SendingUser).
                    ToListAsync());
                scope.Complete();
            }
            catch (Exception) { }
            return await Task.Run(() => arr);
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
