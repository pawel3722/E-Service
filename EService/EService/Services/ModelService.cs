using EService.Dtos.ModelDtos;
using EService.Models;
using EService.Repositories;

namespace EService.Services
{
    public class ModelService : IModelService
    {
        private readonly IModelRepository _modelRepository;
        public ModelService(IModelRepository modelRepository)
        {
            _modelRepository = modelRepository;
        }
        public async Task<List<Model>> GetAllModels()
        {
            return await _modelRepository.GetAllModels();
        }
        public async Task<Model?> GetModel(int id)
        {
            return await _modelRepository.GetModelById(id);
        }
        public async Task<(bool Confirmed, string Response)> CreateModel(ModelDto request)
        {
            var model = await _modelRepository.GetModelByName(request.Name);
            if (model == null)
            {
                model = new Model
                {
                    Name = request.Name,
                    Type = request.Type,
                    Price = request.Price
                };
                await _modelRepository.AddModelAsync(model);
                return await Task.FromResult((true, "Model successfully created."));
            }
            else return await Task.FromResult((false, "Such model already exists."));
        }
        public async Task<(bool Confirmed, string Response)> UpdateModel(ModelDto request, int id)
        {
            var model = await _modelRepository.GetModelById(id);
            if (model != null)
            {
                model.Name = request.Name;
                model.Type = request.Type;
                model.Price = request.Price;
                await _modelRepository.SaveChangesAsync();
                return await Task.FromResult((true, "Model successfully updated."));
            }
            else return await Task.FromResult((false, "Model with given id does not exist."));
        }

        public async Task<(bool Confirmed, string Response)> DeleteModel(int id)
        {
            var model = await _modelRepository.GetModelById(id);
            if (model != null)
            {
                await _modelRepository.RemoveModelAsync(model);
                return await Task.FromResult((true, "Model successfully deleted."));
            }
            else return await Task.FromResult((false, "Model with given id does not exist."));
        }
    }
}
