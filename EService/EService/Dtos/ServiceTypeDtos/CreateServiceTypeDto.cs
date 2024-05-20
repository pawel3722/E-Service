using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace EService.Dtos.ServiceTypeDtos
{
    public class CreateServiceTypeDto
    {
        [Required]
        public string Name { get; set; }
        [Required, Range(0, double.MaxValue)]
        public double MinPrice { get; set; }
        [Required, Range(0, double.MaxValue)]
        public double MaxPrice { get; set; }
        [Required]
        public string DeviceType { get; set; }
    }
}
