using System.ComponentModel.DataAnnotations;

namespace EService.Dtos.ServiceTypeDtos
{
    public class UpdateServiceTypeDto
    {
        public string? Name { get; set; }
        public double? MinPrice { get; set; }
        public double? MaxPrice { get; set; }
    }
}
