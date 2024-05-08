using EService.Models;
namespace EService.Dtos.ReviewDtos
{
    public class ReviewDto
    {
        public double Rating { get; set; }
        public string? Comment { get; set; }
        public int OrderId { get; set; }
    }
}
