using EService.Dtos.ModelDtos;
using EService.Dtos.ServiceDtos;
using EService.Models;

namespace EService.Dtos.PartDtos
{
    public class ReturnPartDto
    {
        public int Id { get; set; }
        public string SerialNumber { get; set; }
        public FieldsOnlyServiceDto? Service { get; set; }
        public int? ModelId { get; set; }
        public FieldsOnlyModelDto? Model { get; set; }
    }
}
