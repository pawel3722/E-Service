using Microsoft.AspNetCore.Mvc.ViewEngines;
using System.ComponentModel.DataAnnotations.Schema;

namespace EService.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        // 0 -> Przyjęto do realizacji 
        // 1 -> Przypisano menadżera
        // 2 -> Ukończono ekspertyzę
        // 3 -> Zlecono wykonanie działań
        // 4 -> Naprawiono
        // 5 -> Odebrano
        public int Status { get; set; }
        public bool Paid { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public ApplicationUser Customer { get; set; }
        [ForeignKey("Manager")]
        public int? ManagerId { get; set; }
        public ApplicationUser? Manager { get; set; }

        public Review? Review { get; set; }

        public List<Service> Services { get; set; } = new List<Service>();

        public Order() { }
    }
}
