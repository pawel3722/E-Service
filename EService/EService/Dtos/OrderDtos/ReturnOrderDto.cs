using EService.Dtos.ApplicationUserDtos;
using EService.Dtos.ReviewDtos;
using EService.Dtos.ServiceDtos;
using EService.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace EService.Dtos.OrderDtos
{
    public class ReturnOrderDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        // 0 -> Przyjęto do realizacji 
        // 1 -> Przypisano menadżera
        // 2 -> Ukończono ekspertyzę
        // 3 -> Zlecono wykonanie działań
        // 4 -> Naprawiono
        // 5 -> Odebrano
        public int Status { get; set; }
        public bool Paid { get; set; }

        [ForeignKey("Customer")] //?
        public int CustomerId { get; set; }
        public FieldsOnlyApplicationUserDto Customer { get; set; }
        [ForeignKey("Manager")]
        public int? ManagerId { get; set; }
        public FieldsOnlyApplicationUserDto? Manager { get; set; }

        public FieldsOnlyReviewDto? Review { get; set; }

        public List<FieldsOnlyServiceDto> Services { get; set; } = new List<FieldsOnlyServiceDto>();
    }
}
