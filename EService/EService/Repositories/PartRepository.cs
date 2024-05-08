using EService.Data;
using EService.Models;

namespace EService.Repositories
{
    public class PartRepository : IPartRepository
    {
        private readonly ApplicationDbContext _context;

        public PartRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Part?> GetPartById(int id)
        {
            return await Task.Run(() => _context.Parts.FindAsync(id).Result);

        }
        public async Task<List<Part>> GetAllParts()
        {
            return await Task.Run(() => _context.Parts.ToList());

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
