using System.ComponentModel.DataAnnotations.Schema;

namespace EService.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime SendingDate { get; set; }

        [ForeignKey("SendingUser")]
        public int? SendingUserId { get; set; }
        public ApplicationUser? SendingUser { get; set; }

        [ForeignKey("ReceivingUser")]
        public int ReceivingUserId { get; set; }
        public ApplicationUser ReceivingUser { get; set; }

        public Message() { }
    }
}
