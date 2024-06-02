using System.Text.Json.Serialization;

namespace EService.Models
{
    public class Service
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
        public ApplicationUser? Serviceman { get; set; }

        public int OrderId { get; set; }
        [JsonIgnore]
        public Order Order { get; set; }

        public int ServiceTypeId { get; set; }
        [JsonIgnore]
        public ServiceType ServiceType { get; set; }

        public int? PartId { get; set; }
        [JsonIgnore]
        public Part? Part { get; set; }

        public Service() { }
    }
}
