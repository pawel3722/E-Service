namespace EService.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }

        /*[InverseProperty("UserId")]*/
        public List<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();

    }
}
