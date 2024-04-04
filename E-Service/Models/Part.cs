namespace E_Service.Models
{
    public class Part
    {
        public int Id { get; set; }
        public int SerialNumber { get; set; }
        public Service Service { get; set; }
        public int ModelId { get; set; }
        public Model Model { get; set; }
        public Part() { }
    }
}
