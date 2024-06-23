using EService.Dtos.ApplicationUserDtos;
using EService.Models;

namespace EService.Dtos.RolesDtos
{
    public class ReturnRoleDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        /*[InverseProperty("UserId")]*/
        //[JsonIgnore]
        public List<FieldsOnlyApplicationUserDto> Users { get; set; } = new List<FieldsOnlyApplicationUserDto>();
    }
}
