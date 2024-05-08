using EService.Dtos.MessageDtos;
using EService.Dtos.PartDtos;
using EService.Models;
using Microsoft.AspNetCore.Mvc;

namespace EService.Services
{
    public interface IPartService
    {

        public Task<Part?> GetPart(int id);
        public Task<List<Part>> GetAllParts();
        public Task<(bool Confirmed, string Response)> CreatePart(PartDto request);
        public Task<(bool Confirmed, string Response)> UpdatePart(PartDto request, int id);
        public Task<(bool Confirmed, string Response)> DeletePart(int id);
    }
}
