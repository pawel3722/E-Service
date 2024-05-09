using EService.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EService.Dtos.MessageDtos
{
    public class CreateMessageDto
    {
        [Required]
        public string Text { get; set; } = string.Empty;
        public DateTime SendingDate { get; set; } = DateTime.Now;
        [Required]
        public int ReceivingUserId { get; set; }
    }
}
