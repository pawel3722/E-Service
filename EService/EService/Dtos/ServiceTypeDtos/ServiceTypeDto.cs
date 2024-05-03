using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace EService.Dtos.ServiceTypeDtos
{
    public class ServiceTypeDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Range(0, double.MaxValue)]
        public double MinPrice { get; set; } = 0.0;
        [Range(0, double.MaxValue)]
        public double MaxPrice { get; set; } = 0.0;
    }
}
