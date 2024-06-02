using EService.Models;

namespace EService.Repositories.Interfaces
{
    public interface IPartRepository
    {

        public Task<Part?> GetPartByIdAsync(int id);
        public Task<Part?> GetPartBySerialNumber(string serialNumber);
        public Task<List<Part>> GetAllPartsAsync();
        public Task AddPartAsync(Part Part);
        public Task SomethingAsync(int id, Service service);
        public Task RemovePartAsync(Part Part);
        public Task SaveChangesAsync();
    }
}
