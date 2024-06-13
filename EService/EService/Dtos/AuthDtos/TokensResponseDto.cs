using EService.Models;

namespace EService.Dtos.AuthDtos
{
    public class TokensResponseDto
    {
        public string JwtToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime Expires { get; set; }
        public List<Role> Roles { get; set; } = new List<Role>();
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public TokensResponseDto() { }
    }
}
