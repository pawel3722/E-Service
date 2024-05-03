using EService.Dtos.ModelDtos;
using EService.Models;

namespace EService.Services
{
    public interface IModelService
    {
        public Task<Model?> GetModel(int id);
        public Task<List<Model>> GetAllModels();
        public Task<(bool Confirmed, string Response)> CreateModel(ModelDto request);
        public Task<(bool Confirmed, string Response)> UpdateModel(ModelDto request, int id);
        public Task<(bool Confirmed, string Response)> DeleteModel(int id);
    }
}
