using System.ComponentModel.DataAnnotations.Schema;

namespace E2_Service.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Status { get; set; }
        public bool Paid { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public ApplicationUser Customer { get; set; }
        [ForeignKey("Manager")]
        public int? ManagerId { get; set; }
        public ApplicationUser? Manager { get; set; }

        public Review Review { get; set; }

        public List<Service> Services { get; set; } = new List<Service>();

        public Order() { }
    }
}
