using System.ComponentModel.DataAnnotations.Schema;

namespace E_Service.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Status { get; set; }
        public bool Paid { get; set; }

        [ForeignKey("Customer")]
        public string CustomerId { get; set; }
        public ApplicationUser Customer { get; set; }
        [ForeignKey("Manager")]
        public string? ManagerId { get; set; }
        public ApplicationUser? Manager { get; set; }

        public Review Review { get; set; }

        public List<Service> Services { get; set; }

        public Order() { }
    }
}
