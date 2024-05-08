using EService.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EService.Dtos.PartDtos

{
    public class PartDto
    {
        public int SerialNumber { get; set; }
        public int ModelId { get; set; }
    }
}
