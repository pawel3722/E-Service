namespace E2_Service.Models
{
    public class ServiceType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double MinPrice { get; set; }
        public double MaxPrice { get; set; }
        public List<Service> Services { get; set; } = new List<Service>();
        public ServiceType() { }
    }
}
