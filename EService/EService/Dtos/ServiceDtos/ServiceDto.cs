using EService.Models;

namespace EService.Dtos.ServiceDtos
{
    public class ServiceDto
    {
        public double PartPrice { get; set; }
        public double ServicePrice { get; set; }
        public int OrderId { get; set; }
        public int ServiceTypeId { get; set; }
        public int PartId { get; set; }
    }
}
