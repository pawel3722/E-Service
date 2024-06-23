namespace EService.Dtos.ReviewDtos
{
    public class FieldsOnlyReviewDto
    {
        public int Id { get; set; }
        public double Rating { get; set; }
        public string? Comment { get; set; }

        public int OrderId { get; set; } //?
    }
}
