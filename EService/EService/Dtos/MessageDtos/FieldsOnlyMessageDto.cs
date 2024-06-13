using EService.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace EService.Dtos.MessageDtos
{
    public class FieldsOnlyMessageDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime SendingDate { get; set; }

        //[ForeignKey("SendingUser")]
        public int? SendingUserId { get; set; }

       // [ForeignKey("ReceivingUser")]
        public int ReceivingUserId { get; set; }

    }
}
