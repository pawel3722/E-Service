using EService.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EService.Dtos.MessageDtos
{
    public class MessageDto
    {
        [Required]
        public string Text { get; set; } = string.Empty;
        [Required]
        public DateTime SendingDate { get; set; }
        [Required]
        public DateTime ReceivingDate { get; set; }
        public int? SendingUserId { get; set; }
        [Required]
        public int ReceivingUserId { get; set; }
    }
}
