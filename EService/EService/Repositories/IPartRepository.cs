using EService.Models;

namespace EService.Repositories
{
    public interface IPartRepository
    {

        public Task<Part?> GetPartById(int id);
        public Task<List<Part>> GetAllParts();
        public Task AddPartAsync(Part Part);
        public Task RemovePartAsync(Part Part);
        public Task SaveChangesAsync();
    }
}
