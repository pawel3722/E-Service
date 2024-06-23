using EService.Data;
using EService.Models;
using EService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace EService.Repositories
{
    public class PartRepository : IPartRepository
    {
        private readonly ApplicationDbContext _context;

        public PartRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Part?> GetPartByIdAsync(int id)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                                   new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                                   TransactionScopeAsyncFlowOption.Enabled);
            Part? part = null;
            try
            {
                part = await _context.Parts.Where(p => p.Id == id).
                    Include(p => p.Service).
                    Include(p => p.Model).
                    FirstOrDefaultAsync();
                scope.Complete();
            }
            catch (Exception) { }
            return await Task.Run(() => part);
        }
        public async Task<Part?> GetPartBySerialNumber(string serialNumber)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                                   new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                                   TransactionScopeAsyncFlowOption.Enabled);
            Part? part = null;
            try
            {
                part = await _context.Parts.Where(p => p.SerialNumber == serialNumber).
                    Include(p => p.Service).
                    Include(p => p.Model).
                    FirstOrDefaultAsync();
                scope.Complete();
            }
            catch (Exception) { }
            return await Task.Run(() => part);
        }
        public async Task<List<Part>> GetAllPartsAsync()
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                                   new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                                   TransactionScopeAsyncFlowOption.Enabled);
            List<Part> arr = new List<Part>();
            try
            {
                arr = await _context.Parts.
                    Include(p => p.Service).
                    Include(p => p.Model).
                    ToListAsync();
                scope.Complete();
            }
            catch (Exception) { }
            return await Task.Run(() => arr);
        }
        public async Task AddPartAsync(Part part)
        {
            await _context.AddAsync(part);
            await SaveChangesAsync();
        }
        public async Task RemovePartAsync(Part part)
        {
            _context.Remove(part);
            await SaveChangesAsync();
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
