using System.ComponentModel.DataAnnotations;

namespace EService.Dtos.ModelDtos
{
    public class ModelDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Type { get; set; } = string.Empty;
        [Range(0, double.MaxValue)]
        public double Price { get; set; } = 0.0;
    }
}
