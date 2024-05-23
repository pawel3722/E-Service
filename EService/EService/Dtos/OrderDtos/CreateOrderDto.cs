using EService.Dtos.ServiceDtos;
using EService.Models;
using System.ComponentModel.DataAnnotations;

namespace EService.Dtos.OrderDtos
{
    public class CreateOrderDto
    {
        public DateTime Date { get; set; } = DateTime.Now;
        [Required]
        public bool Paid { get; set; }
        [Required]
        public int CustomerId { get; set; }
        public int? ManagerId { get; set; }
        public List<CreateServiceDto> Services { get; set; } = new List<CreateServiceDto>();
    }
}
