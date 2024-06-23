using EService.Models;

namespace EService.Dtos.PartDtos
{
    public class FieldsOnlyPartDto
    {
        public int Id { get; set; }
        public string SerialNumber { get; set; }
        public int? ModelId { get; set; } //?
    }
}
