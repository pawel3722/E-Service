using System.ComponentModel.DataAnnotations;

namespace EService.Dtos.ModelDtos
{
    public class UpdateModelDto
    {
        public string? Name { get; set; }
        public string? Type { get; set; }
        [Range(0, double.MaxValue)]
        public double? Price { get; set; }
    }
}
