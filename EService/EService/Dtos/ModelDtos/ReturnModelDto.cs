using EService.Dtos.PartDtos;
using EService.Models;

namespace EService.Dtos.ModelDtos
{
    public class ReturnModelDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public double Price { get; set; }
        public List<FieldsOnlyPartDto> Parts { get; set; } = new List<FieldsOnlyPartDto>();
    }
}
