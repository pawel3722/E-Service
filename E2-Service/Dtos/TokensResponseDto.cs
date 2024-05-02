namespace E2_Service.Dtos
{
    public class TokensResponseDto
    {
        public string JwtToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime Expires {  get; set; }
    }
}
