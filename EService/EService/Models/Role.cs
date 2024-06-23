using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EService.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }

        /*[InverseProperty("UserId")]*/
        //[JsonIgnore]
        public List<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();

    }
}
