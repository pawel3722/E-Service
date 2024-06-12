using EService.Data;
using EService.Models;
using EService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

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
            return await Task.Run(() => _context.Parts.
            Include(p => p.Service).
            Include(p => p.Model).
            FirstOrDefaultAsync(p => p.Id == id));

        }
        public async Task<Part?> GetPartBySerialNumber(string serialNumber)
        {
            return await Task.Run(() => _context.Parts.
            Include(p => p.Service).
            Include(p => p.Model).
            FirstOrDefaultAsync(p => p.SerialNumber == serialNumber));
        }
        public async Task<List<Part>> GetAllPartsAsync()
        {
            return await Task.Run(() => _context.Parts.
            Include(p => p.Service).
            Include(p => p.Model).
            ToListAsync());

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
