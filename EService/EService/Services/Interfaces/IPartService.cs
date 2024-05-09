using EService.Dtos.MessageDtos;
using EService.Dtos.PartDtos;
using EService.Models;
using Microsoft.AspNetCore.Mvc;

namespace EService.Services
{
    public interface IPartService
    {

        public Task<Part?> GetPartAsync(int id);
        public Task<List<Part>> GetAllPartsAsync();
        public Task<(bool Confirmed, string Response)> CreatePartAsync(CreatePartDto request);
        public Task<(bool Confirmed, string Response)> UpdatePartAsync(UpdatePartDto request, int id);
        public Task<(bool Confirmed, string Response)> DeletePartAsync(int id);
    }
}
