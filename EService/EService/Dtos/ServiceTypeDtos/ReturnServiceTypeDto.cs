using EService.Dtos.ServiceDtos;
using EService.Models;

namespace EService.Dtos.ServiceTypeDtos
{
    public class ReturnServiceTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double MinPrice { get; set; }
        public double MaxPrice { get; set; }
        public string DeviceType { get; set; }
        public List<FieldsOnlyServiceDto> Services { get; set; } = new List<FieldsOnlyServiceDto>();
    }
}
