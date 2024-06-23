using AutoMapper;
using EService.Dtos.ApplicationUserDtos;
using EService.Dtos.PartDtos;
using EService.Models;
using EService.Repositories;
using EService.Repositories.Interfaces;

namespace EService.Services
{
    public class PartService : IPartService
    {
        private readonly IPartRepository _partRepository;
        private readonly IModelRepository _modelRepository;
        private readonly IMapper _mapper;
        public PartService(IPartRepository partRepository, IModelRepository modelRepository, IMapper mapper)
        {
            _partRepository = partRepository;
            _modelRepository = modelRepository;
            _mapper = mapper;
        }

        public async Task<List<ReturnPartDto>> GetAllPartsAsync()
        {
            var parts = await _partRepository.GetAllPartsAsync();
            return _mapper.Map<List<ReturnPartDto>>(parts);
           // return await _partRepository.GetAllPartsAsync();
        }
        public async Task<ReturnPartDto?> GetPartAsync(int id)
        {
            var part = await _partRepository.GetPartByIdAsync(id);
            return _mapper.Map<ReturnPartDto>(part);
            //return await _partRepository.GetPartByIdAsync(id);
        }
        public async Task<(bool Confirmed, string Response)> CreatePartAsync(CreatePartDto request)
        {
            Model? model = null;
            if (request.ModelId != null)
            {
                model = await _modelRepository.GetModelByIdAsync(request.ModelId.Value);
                if (model == null) return await Task.FromResult((false, "Model with given id does not exist."));
            }
            var part = new Part
            {
                SerialNumber = request.SerialNumber,
                ModelId = request.ModelId,
                Model = model
            };
            if(model != null) model.Parts.Add(part);
            await _partRepository.AddPartAsync(part);
            return await Task.FromResult((true, "Part successfully created."));  
        }
        public async Task<(bool Confirmed, string Response)> UpdatePartAsync(UpdatePartDto request, int id)
        {
            var part = await _partRepository.GetPartByIdAsync(id);
            if (part == null) return await Task.FromResult((false, "Part with given id does not exist."));
            Model? model = null;
            if (request.ModelId != null)
            {
                model = await _modelRepository.GetModelByIdAsync(request.ModelId.Value);
                if(model == null) return await Task.FromResult((false, "Model with given id does not exist."));
                if(part.Model != null) part.Model.Parts.Remove(part);
                part.ModelId = request.ModelId.Value;
                part.Model = model;
            }
            if(request.SerialNumber != null) part.SerialNumber = request.SerialNumber;
            await _partRepository.SaveChangesAsync();
            return await Task.FromResult((true, "Part successfully updated."));
        }

        public async Task<(bool Confirmed, string Response)> DeletePartAsync(int id)
        {
            var part = await _partRepository.GetPartByIdAsync(id);
            if (part != null) return await Task.FromResult((false, "Part with given id does not exist."));
            await _partRepository.RemovePartAsync(part);
            return await Task.FromResult((true, "Part successfully deleted."));
        }


    }
}
