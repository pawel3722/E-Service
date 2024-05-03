using System.ComponentModel.DataAnnotations;

namespace EService.Dtos
{
    public class UserRegisterRequestDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Surname { get; set; } = string.Empty;
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required, RegularExpression("^(?=.{8,})(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%^&+=_]).*$",
            ErrorMessage = "Password must contain at least eight characters and one lowercase letter, one uppercase letter, one special character and one digit.")]
        public string Password { get; set; } = string.Empty;
        [Required, Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
