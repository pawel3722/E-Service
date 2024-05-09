using EService.Models;

namespace EService.Repositories.Interfaces
{
    public interface IModelRepository
    {
        public Task<Model?> GetModelByNameAsync(string name);
        public Task<Model?> GetModelByIdAsync(int id);
        public Task<List<Model>> GetAllModelsAsync();
        public Task AddModelAsync(Model model);
        public Task RemoveModelAsync(Model model);
        public Task SaveChangesAsync();
    }
}
