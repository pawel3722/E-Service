using System.ComponentModel.DataAnnotations;

namespace EService.Dtos.MessageDtos
{
    public class UpdateMessageDto
    {
        public string? Text { get; set; }
        public DateTime? SendingDate { get; set; }
    }
}
