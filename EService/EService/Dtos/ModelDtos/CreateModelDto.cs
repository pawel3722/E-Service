using EService.Dtos.PartDtos;
using System.ComponentModel.DataAnnotations;

namespace EService.Dtos.ModelDtos
{
    public class CreateModelDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Type { get; set; } = string.Empty;
        [Required, Range(0, double.MaxValue)]
        public double Price { get; set; } = 0.0;
        public List<CreatePartDto> Parts { get; set; } = new List<CreatePartDto>();
    }
}
