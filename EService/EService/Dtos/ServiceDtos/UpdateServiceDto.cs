using System.ComponentModel.DataAnnotations;

namespace EService.Dtos.ServiceDtos
{
    public class UpdateServiceDto
    {
        public int? Status { get; set; }
        public DateTime? Guarantee { get; set; }
        public DateTime? Date { get; set; }
        public double? ServicePrice { get; set; }
        public int? ServicemanId { get; set; }
        public int? OrderId { get; set; }
        public int? ServiceTypeId { get; set; }
        public int? PartId { get; set; }
    }
}
