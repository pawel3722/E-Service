using EService.Dtos.MessageDtos;
using EService.Dtos.OrderDtos;
using EService.Dtos.RolesDtos;
using EService.Dtos.ServiceDtos;
using EService.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace EService.Dtos.ApplicationUserDtos
{
    public class ReturnApplicationUserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
       
        //USUNELAM HASLA ITP

        /*[InverseProperty("RoleId")]*/
        public List<FieldsOnlyRoleDto> Roles { get; set; } = new List<FieldsOnlyRoleDto>();

       // [InverseProperty("SendingUser")] wywalic
        public List<FieldsOnlyMessageDto> SentMessages { get; set; } = new List<FieldsOnlyMessageDto>();
        [InverseProperty("ReceivingUser")]
        public List<FieldsOnlyMessageDto> ReceivedMessages { get; set; } = new List<FieldsOnlyMessageDto>();
        [InverseProperty("Customer")]
        public List<FieldsOnlyOrderDto> CustomerOrders { get; set; } = new List<FieldsOnlyOrderDto>();
        [InverseProperty("Manager")]
        public List<FieldsOnlyOrderDto> ManagerOrders { get; set; } = new List<FieldsOnlyOrderDto>();
        [InverseProperty("Serviceman")]
        public List<FieldsOnlyServiceDto> Services { get; set; } = new List<FieldsOnlyServiceDto>();
    }
}
