using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Text.Json.Serialization;

namespace EService.Models
{
    public class ApplicationUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        [JsonIgnore]
        public byte[] PasswordHash { get; set; }
        [JsonIgnore]
        public byte[] PasswordSalt { get; set; }
        [JsonIgnore]
        public string RefreshToken { get; set; } = string.Empty;
        [JsonIgnore]
        public DateTime TokenCreated { get; set; }
        [JsonIgnore]
        public DateTime TokenExpires { get; set; }

        /*[InverseProperty("RoleId")]*/
        public List<Role> Roles { get; set; } = new List<Role>();

        [InverseProperty("SendingUser"), JsonIgnore]
        public List<Message> SentMessages { get; set; } = new List<Message>();
        [InverseProperty("ReceivingUser"), JsonIgnore]
        public List<Message> ReceivedMessages { get; set; } = new List<Message>();
        [InverseProperty("Customer")]
        public List<Order> CustomerOrders { get; set; } = new List<Order>();
        [InverseProperty("Manager")]
        public List<Order> ManagerOrders { get; set; } = new List<Order>();
        public List<Service> Services { get; set; } = new List<Service>();

        public ApplicationUser() { }

    }
}
