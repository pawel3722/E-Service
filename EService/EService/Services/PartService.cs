using EService.Dtos.PartDtos;
using EService.Models;
using EService.Repositories;

namespace EService.Services
{
    public class PartService : IPartService
    {
        private readonly IPartRepository _partRepository;
        public PartService(IPartRepository partRepository)
        {
            _partRepository = partRepository;
        }



        public async Task<List<Part>> GetAllParts()
        {
            return await _partRepository.GetAllParts();
        }
        public async Task<Part?> GetPart(int id)
        {
            return await _partRepository.GetPartById(id);
        }
        public async Task<(bool Confirmed, string Response)> CreatePart(PartDto request)
        {
            var part = new Part
            {
                SerialNumber = request.SerialNumber,
                ModelId = request.ModelId
            };
            await _partRepository.AddPartAsync(part);
            return await Task.FromResult((true, "Part successfully created."));
        }
        public async Task<(bool Confirmed, string Response)> UpdatePart(PartDto request, int id)
        {
            var part = await _partRepository.GetPartById(id);
            if (part != null)
            {
                part.SerialNumber = request.SerialNumber;
                part.ModelId = request.ModelId;
                await _partRepository.SaveChangesAsync();
                return await Task.FromResult((true, "Part successfully updated."));
            }
            else return await Task.FromResult((false, "Part with given id does not exist."));
        }

        public async Task<(bool Confirmed, string Response)> DeletePart(int id)
        {
            var part = await _partRepository.GetPartById(id);
            if (part != null)
            {
                await _partRepository.RemovePartAsync(part);
                return await Task.FromResult((true, "Part successfully deleted."));
            }
            else return await Task.FromResult((false, "Part with given id does not exist."));
        }


    }
}
