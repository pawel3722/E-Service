using EService.Data;
using EService.Models;
using EService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EService.Repositories
{
    public class ModelRepository : IModelRepository
    {
        private readonly ApplicationDbContext _context;

        public ModelRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Model?> GetModelByNameAsync(string name)
        {
            return await Task.Run(() => _context.Models.Where(m => m.Name == name).
            Include(m => m.Parts).
            FirstOrDefaultAsync());
        }
        public async Task<Model?> GetModelByIdAsync(int id)
        {
            return await Task.Run(() => _context.Models.
            Include(m => m.Parts).
            FirstOrDefaultAsync(m => m.Id == id));
        }
        public async Task<List<Model>> GetAllModelsAsync()
        {
            return await Task.Run(() => _context.Models.
            Include(m => m.Parts).
            ToListAsync());
        }
        public async Task AddModelAsync(Model model)
        {
            await _context.AddAsync(model);
            await SaveChangesAsync();
        }
        public async Task RemoveModelAsync(Model model)
        {
            _context.Remove(model);
            await SaveChangesAsync();
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
