using EService.Dtos.ReviewDtos;
using EService.Dtos.ServiceDtos;
using System.ComponentModel.DataAnnotations;

namespace EService.Dtos.OrderDtos
{
    public class UpdateOrderDto
    {
        public DateTime? Date { get; set; }
        public int? Status { get; set; }
        public bool? Paid { get; set; }
        public int? CustomerId { get; set; }
        public int? ManagerId { get; set; }
        public CreateReviewDto? CreateReviewDto { get; set; }
        public List<CreateServiceDto> newServices { get; set; } = new List<CreateServiceDto>();
    }
}
