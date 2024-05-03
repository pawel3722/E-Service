namespace EService.Models
{
    public class Review
    {
        public int Id { get; set; }
        public double Rating { get; set; }
        public string? Comment { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }
        public Review() { }
    }
}
