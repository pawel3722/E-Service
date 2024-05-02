using System.ComponentModel.DataAnnotations;

namespace E2_Service.Dtos
{
    public class UserLoginRequestDto
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
