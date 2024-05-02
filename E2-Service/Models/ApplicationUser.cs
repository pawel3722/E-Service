using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace E2_Service.Models
{
    public class ApplicationUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime TokenCreated { get; set; }
        public DateTime TokenExpires { get; set; }

        /*[InverseProperty("RoleId")]*/
        public List<Role> Roles { get; set; } = new List<Role>();

        [InverseProperty("SendingUser")]
        public List<Message> SentMessages { get; set; } = new List<Message>();
        [InverseProperty("ReceivingUser")]
        public List<Message> ReceivedMessages { get; set; } = new List<Message>();
        [InverseProperty("Customer")]
        public List<Order>? CustomerOrders { get; set; } = new List<Order>();
        [InverseProperty("Manager")]
        public List<Order>? ManagerOrders { get; set; } = new List<Order>();
        public List<Service>? Services { get; set; } = new List<Service>();

        public ApplicationUser() { }

    }
}
