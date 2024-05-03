using EService.Data;
using EService.Models;
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
        public async Task<Model?> GetModelByName(string name)
        {
            return await Task.Run(() => _context.Models.Where(m => m.Name == name).FirstOrDefaultAsync());
        }
        public async Task<Model?> GetModelById(int id)
        {
            return await Task.Run(() => _context.Models.FindAsync(id).Result);
        }
        public async Task<List<Model>> GetAllModels()
        {
            return await Task.Run(() => _context.Models.ToList());
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
