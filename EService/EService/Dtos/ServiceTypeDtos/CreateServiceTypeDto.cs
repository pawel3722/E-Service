using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace EService.Dtos.ServiceTypeDtos
{
    public class CreateServiceTypeDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required, Range(0, double.MaxValue)]
        public double MinPrice { get; set; } = 0.0;
        [Required, Range(0, double.MaxValue)]
        public double MaxPrice { get; set; } = 0.0;
    }
}
