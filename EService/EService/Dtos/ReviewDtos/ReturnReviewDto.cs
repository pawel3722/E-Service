using EService.Dtos.OrderDtos;
using EService.Models;

namespace EService.Dtos.ReviewDtos
{
    public class ReturnReviewDto
    {
        public int Id { get; set; }
        public double Rating { get; set; }
        public string? Comment { get; set; }

        public int OrderId { get; set; }
        public FieldsOnlyOrderDto Order { get; set; }
    }
}
