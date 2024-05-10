using EService.Dtos.ModelDtos;
using EService.Dtos.PartDtos;
using EService.Models;
using EService.Repositories.Interfaces;

namespace EService.Services
{
    public class ModelService : IModelService
    {
        private readonly IModelRepository _modelRepository;
        public ModelService(IModelRepository modelRepository)
        {
            _modelRepository = modelRepository;
        }
        public async Task<List<Model>> GetAllModelsAsync()
        {
            return await _modelRepository.GetAllModelsAsync();
        }
        public async Task<Model?> GetModelAsync(int id)
        {
            return await _modelRepository.GetModelByIdAsync(id);
        }
        public async Task<(bool Confirmed, string Response)> CreateModelAsync(CreateModelDto request)
        {
            var model = await _modelRepository.GetModelByNameAsync(request.Name);
            if (model == null)
            {
                var listOfParts = new List<Part>();
                foreach(var partDto in request.Parts)
                {
                    listOfParts.Add(new Part()
                    {
                        SerialNumber = partDto.SerialNumber
                    });
                }
                model = new Model
                {
                    Name = request.Name,
                    Type = request.Type,
                    Price = request.Price,
                    Parts = listOfParts
                };
                await _modelRepository.AddModelAsync(model);
                model = await _modelRepository.GetModelByIdAsync(model.Id);
                foreach(var part in model!.Parts)
                {
                    part.ModelId = model.Id;
                    part.Model = model;
                }
                await _modelRepository.SaveChangesAsync();
                return await Task.FromResult((true, "Model successfully created."));
            }
            else return await Task.FromResult((false, "Model with given name already exists."));
        }
        public async Task<(bool Confirmed, string Response)> UpdateModelAsync(UpdateModelDto request, int id)
        {
            var model = await _modelRepository.GetModelByIdAsync(id);
            if (model != null)
            {
                if (request.Name != null) model.Name = request.Name!;
                if (request.Type != null) model.Type = request.Type!;
                if (request.Price != null) model.Price = request.Price.Value;
                await _modelRepository.SaveChangesAsync();
                return await Task.FromResult((true, "Model successfully updated."));
            }
            else return await Task.FromResult((false, "Model with given id does not exist."));
        }

        public async Task<(bool Confirmed, string Response)> DeleteModelAsync(int id)
        {
            var model = await _modelRepository.GetModelByIdAsync(id);
            if (model != null)
            {
                await _modelRepository.RemoveModelAsync(model);
                return await Task.FromResult((true, "Model successfully deleted."));
            }
            else return await Task.FromResult((false, "Model with given id does not exist."));
        }
    }
}
