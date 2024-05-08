using EService.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace EService.Dtos.OrderDtos
{
    public class OrderDto
    {
        public DateTime Date { get; set; }
        public int Status { get; set; }
        public bool Paid { get; set; }
        public int CustomerId { get; set; }
        //public List<Service> Services { get; set; } = new List<Service>();


    }
}
