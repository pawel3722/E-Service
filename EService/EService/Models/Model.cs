namespace EService.Models
{
    public class Model
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public double Price { get; set; }
        public List<Part> Parts { get; set; } = new List<Part>();

        public Model() { }
    }
}
