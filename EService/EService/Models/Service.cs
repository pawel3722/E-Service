namespace EService.Models
{
    public class Service
    {
        public int Id { get; set; }
        public int Status { get; set; }
        public DateTime Guarantee { get; set; }
        public DateTime Date { get; set; }
        public double PartPrice { get; set; }
        public double ServicePrice { get; set; }

        public int? ServicemanId { get; set; }
        public ApplicationUser? Serviceman { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }

        public int ServiceTypeId { get; set; }
        public ServiceType ServiceType { get; set; }

        public int PartId { get; set; }
        public Part Part { get; set; }

        public Service() { }
    }
}
