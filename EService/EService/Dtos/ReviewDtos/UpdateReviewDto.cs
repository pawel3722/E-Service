using System.ComponentModel.DataAnnotations;

namespace EService.Dtos.ReviewDtos
{
    public class UpdateReviewDto
    {
        public double? Rating { get; set; }
        public string? Comment { get; set; }
        public int? OrderId { get; set; }
    }
}
