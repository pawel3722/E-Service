using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace E_Service.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }

        [InverseProperty("SendingUser")]
        public List<Message> SentMessages { get; set; }
        [InverseProperty("ReceivingUser")]
        public List<Message> ReceivedMessages { get; set; }
        [InverseProperty("Customer")]
        public List<Order>? CustomerOrders { get; set; }
        [InverseProperty("Manager")]
        public List<Order>? ManagerOrders { get; set; }
        public List<Service>? Services { get; set; }

        public ApplicationUser() { }
         
    }
}