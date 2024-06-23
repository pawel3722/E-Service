using EService.Models;

namespace EService.Dtos.ServiceDtos
{
    public class FieldsOnlyServiceDto
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

        //ID ZOSTAWIC?
        public int? ServicemanId { get; set; }

        public int OrderId { get; set; }
       
        public int ServiceTypeId { get; set; }
  
        public int? PartId { get; set; }

    }
}
