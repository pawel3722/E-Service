using System.ComponentModel.DataAnnotations.Schema;

namespace E_Service.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime SendingDate { get; set; }
        public DateTime ReceivingDate { get; set; }

        [ForeignKey("SendingUser")]
        public string? SendingUserId { get; set; }
        public ApplicationUser? SendingUser { get; set; }
        
        [ForeignKey("ReceivingUser")]
        public string ReceivingUserId {  get; set; } 
        public ApplicationUser ReceivingUser { get; set; }
        

        public Message() { }
    }
}
