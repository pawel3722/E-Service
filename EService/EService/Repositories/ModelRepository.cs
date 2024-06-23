using EService.Data;
using EService.Models;
using EService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

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
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                                    new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                                    TransactionScopeAsyncFlowOption.Enabled);
            Model? model = null;
            try
            {
                model = await Task.Run(() => _context.Models.Where(m => m.Name == name).
                    Include(m => m.Parts).
                    FirstOrDefaultAsync());
                scope.Complete();
            }
            catch (Exception) { }
            return await Task.Run(() => model);
        }
        public async Task<Model?> GetModelByIdAsync(int id)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                                    new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                                    TransactionScopeAsyncFlowOption.Enabled);
            Model? model = null;
            try
            {
                model = await Task.Run(() => _context.Models.
                    Include(m => m.Parts).
                    FirstOrDefaultAsync(m => m.Id == id));
                scope.Complete();
            }
            catch (Exception) { }
            return await Task.Run(() => model);
        }
        public async Task<List<Model>> GetAllModelsAsync()
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                                    new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                                    TransactionScopeAsyncFlowOption.Enabled);
            List<Model> arr = new List<Model>();
            try
            {
                arr = await Task.Run(() => _context.Models.
                    Include(m => m.Parts).
                    ToListAsync());
                scope.Complete();
            }
            catch (Exception) { }
            return await Task.Run(() => arr);
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
