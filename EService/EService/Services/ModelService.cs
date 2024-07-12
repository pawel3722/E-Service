using AutoMapper;
using EService.Dtos.ModelDtos;
using EService.Dtos.PartDtos;
using EService.Models;
using EService.Repositories.Interfaces;

namespace EService.Services
{
    public class ModelService : IModelService
    {
        private readonly IModelRepository _modelRepository;
        private readonly IMapper _mapper;
        public ModelService(IModelRepository modelRepository, IMapper mapper)
        {
            _modelRepository = modelRepository;
            _mapper = mapper;
        }
        public async Task<List<ReturnModelDto>> GetAllModelsAsync()
        {
            var models = await _modelRepository.GetAllModelsAsync();
            return _mapper.Map<List<ReturnModelDto>>(models);
        }
        public async Task<ReturnModelDto?> GetModelAsync(int id)
        {
            var model = await _modelRepository.GetModelByIdAsync(id);
            return _mapper.Map<ReturnModelDto>(model);
        }
        public async Task<(bool Confirmed, string Response)> CreateModelAsync(CreateModelDto request)
        {
            var model = await _modelRepository.GetModelByNameAsync(request.Name);
            if (model != null) return await Task.FromResult((false, "Model with given name already exists."));
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
            return await Task.FromResult((true, "Model successfully created."));
        }
        public async Task<(bool Confirmed, string Response)> UpdateModelAsync(UpdateModelDto request, int id)
        {
            var model = await _modelRepository.GetModelByIdAsync(id);
            if (model == null) return await Task.FromResult((false, "Model with given id does not exist."));
            if (!(request.Name != null && request.Name != model.Name 
                || request.Type != null && request.Type != model.Type 
                || request.Price != null && request.Price != model.Price))
                return await Task.FromResult((false, "No fields to be updated."));
            if (request.Name != null) model.Name = request.Name!;
            if (request.Type != null) model.Type = request.Type!;
            if (request.Price != null) model.Price = request.Price.Value;
            await _modelRepository.SaveChangesAsync();
            return await Task.FromResult((true, "Model successfully updated."));
        }
        public async Task<(bool Confirmed, string Response)> DeleteModelAsync(int id)
        {
            var model = await _modelRepository.GetModelByIdAsync(id);
            if (model == null) return await Task.FromResult((false, "Model with given id does not exist."));
            await _modelRepository.RemoveModelAsync(model);
            return await Task.FromResult((true, "Model successfully deleted."));
        }
    }
}
