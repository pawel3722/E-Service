using EService.Dtos.ModelDtos;
using EService.Models;

namespace EService.Services
{
    public interface IModelService
    {
        public Task<Model?> GetModelAsync(int id);
        public Task<List<Model>> GetAllModelsAsync();
        public Task<(bool Confirmed, string Response)> CreateModelAsync(CreateModelDto request);
        public Task<(bool Confirmed, string Response)> UpdateModelAsync(UpdateModelDto request, int id);
        public Task<(bool Confirmed, string Response)> DeleteModelAsync(int id);
    }
}
