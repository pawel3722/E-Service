using EService.Models;

namespace EService.Repositories
{
    public interface IModelRepository
    {
        public Task<Model?> GetModelByName(string name);
        public Task<Model?> GetModelById(int id);
        public Task<List<Model>> GetAllModels();
        public Task AddModelAsync(Model model);
        public Task RemoveModelAsync(Model model);
        public Task SaveChangesAsync();
    }
}
