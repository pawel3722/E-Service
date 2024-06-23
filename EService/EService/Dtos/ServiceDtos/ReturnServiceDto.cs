using EService.Dtos.ApplicationUserDtos;
using EService.Dtos.OrderDtos;
using EService.Dtos.PartDtos;
using EService.Dtos.ServiceTypeDtos;
using EService.Models;

namespace EService.Dtos.ServiceDtos
{
    public class ReturnServiceDto
    {
        public int Id { get; set; }
        // 0 -> Utworzono
        // 1 -> Przypisano pracownika
        // 2 -> (awaryjnie) Oczekiwanie na część
        // 3 -> Ukończono
        public int Status { get; set; }
        public DateTime? Guarantee { get; set; }
        public DateTime? Date { get; set; }
        public double PartPrice { get; set; }
        public double ServicePrice { get; set; }

        public int? ServicemanId { get; set; }
        public FieldsOnlyApplicationUserDto? Serviceman { get; set; }

        public int OrderId { get; set; }
        //[JsonIgnore]
        public FieldsOnlyOrderDto Order { get; set; }

        public int ServiceTypeId { get; set; }
        //[JsonIgnore]
        public FieldsOnlyServiceTypeDto ServiceType { get; set; }

        public int? PartId { get; set; }
        //[JsonIgnore]
        public FieldsOnlyPartDto? Part { get; set; }
    }
}
