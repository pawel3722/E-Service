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
            return await Task.Run(() => _context.Parts.Include(p => p.Service).FirstOrDefaultAsync(p => p.Id == id));

        }
        public async Task<Part?> GetPartBySerialNumber(string serialNumber)
        {
            return await Task.Run(() => _context.Parts.Include(p => p.Service).FirstOrDefaultAsync(p => p.SerialNumber == serialNumber));
        }
        public async Task<List<Part>> GetAllPartsAsync()
        {
            return await Task.Run(() => _context.Parts.Include(p => p.Service).ToListAsync());
        }
        public async Task SomethingAsync(int id, Service service)
        {
            var part = await _context.Parts.Where(p => p.Id == id).Include(p => p.Service).FirstOrDefaultAsync();
            if (part == null) return;
            if (part.Service == null)
            {
                part.Service = service;
                await SaveChangesAsync();
            }
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
