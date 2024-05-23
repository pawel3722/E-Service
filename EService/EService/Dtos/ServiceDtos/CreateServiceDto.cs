using EService.Models;
using System.ComponentModel.DataAnnotations;

namespace EService.Dtos.ServiceDtos
{
    public class CreateServiceDto
    {
        [Required]
        public double ServicePrice { get; set; }
        [Required]
        public int OrderId { get; set; }
        [Required]
        public int ServiceTypeId { get; set; }
        public int? PartId { get; set; }
    }
}
