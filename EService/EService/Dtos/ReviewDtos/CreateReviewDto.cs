using EService.Models;
using System.ComponentModel.DataAnnotations;
namespace EService.Dtos.ReviewDtos
{
    public class CreateReviewDto
    {
        [Required]
        public double Rating { get; set; }
        public string? Comment { get; set; }
        [Required]
        public int OrderId { get; set; }
    }
}
