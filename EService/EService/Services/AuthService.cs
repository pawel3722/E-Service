using EService.Dtos;
using EService.Models;
using EService.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace EService.Services
{
    public class AuthService : IAuthService
    {
        private readonly IEServiceRepository _repository;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(IEServiceRepository repository, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<(bool Confirmed, string Response)> RegisterUser(UserRegisterRequestDto request)
        {
            if (!await _repository.UserExists(request.Email))
            {
                var role = await _repository.GetRole("Client");
                CreatePasswordHash(request.Password, out byte[] PasswordHash, out byte[] PasswordSalt);
                var newUser = new ApplicationUser
                {
                    Name = request.Name,
                    Surname = request.Surname,
                    Email = request.Email,
                    PasswordHash = PasswordHash,
                    PasswordSalt = PasswordSalt,
                    Roles = new List<Role> { role! }
                };
                await _repository.AddUser(newUser);
                return await Task.FromResult((true, "User has been succesfully created."));
            }
            else return await Task.FromResult((false, "User with specified email already exists."));
        }

        public async Task<(bool Confirmed, string Response, TokensResponseDto? Tokens)> LoginUser(UserLoginRequestDto request)
        {
            var user = await _repository.GetUserWithEmail(request.Email);
            if (user != null)
            {
                if (VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
                {
                    var jwtToken = CreateJwtToken(user);
                    var refreshToken = GenerateRefreshToken();
                    var tokens = new TokensResponseDto
                    {
                        JwtToken = jwtToken,
                        RefreshToken = refreshToken.Token,
                        CreatedAt = refreshToken.CreatedAt,
                        Expires = refreshToken.Expires
                    };
                    SetRefreshTokenInResponse(refreshToken);
                    await SetRefreshTokenForUser(refreshToken, user);
                    return await Task.FromResult<(bool Confirmed, string Response, TokensResponseDto? Tokens)>((true, $"Welcome {user.Name}.", tokens));
                }
                else return await Task.FromResult<(bool Confirmed, string Response, TokensResponseDto? Tokens)>((false, "Incorrect email or password.", null));
            }
            else return await Task.FromResult<(bool Confirmed, string Response, TokensResponseDto? Tokens)>((false, "Incorrect email or password.", null));
        }

        public async Task<(bool Confirmed, string Response, TokensResponseDto? Tokens)> RefreshToken()
        {
            var refreshToken = _httpContextAccessor.HttpContext!.Request.Cookies["refreshToken"];
            var user = await _repository.GetUserWithRefreshToken(refreshToken!);
            if (user != null)
            {
                if (user.TokenExpires > DateTime.Now)
                {
                    var jwtToken = CreateJwtToken(user);
                    var newRefreshToken = GenerateRefreshToken();
                    var tokens = new TokensResponseDto
                    {
                        JwtToken = jwtToken,
                        RefreshToken = newRefreshToken.Token,
                        CreatedAt = newRefreshToken.CreatedAt,
                        Expires = newRefreshToken.Expires
                    };
                    SetRefreshTokenInResponse(newRefreshToken);
                    await SetRefreshTokenForUser(newRefreshToken, user);
                    return await Task.FromResult<(bool Confirmed, string Response, TokensResponseDto? Tokens)>((true, $"Welcome {user.Name}.", tokens));
                }
                else return await Task.FromResult<(bool Confirmed, string Response, TokensResponseDto? Tokens)>((false, "Token expired.", null));
            }
            else return await Task.FromResult<(bool Confirmed, string Response, TokensResponseDto? Tokens)>((false, "Invalid refresh token.", null));
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
        private string CreateJwtToken(ApplicationUser user)
        {
            var roles = user.Roles.ToList();
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email),
            };
            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role.Name));
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(claims: claims, expires: DateTime.Now.AddHours(1), signingCredentials: creds);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
        private (string Token, DateTime CreatedAt, DateTime Expires) GenerateRefreshToken()
        {
            return (Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)), DateTime.Now, DateTime.Now.AddDays(7));
        }
        private void SetRefreshTokenInResponse((string Token, DateTime CreatedAt, DateTime Expires) refreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = refreshToken.Expires,
            };
            _httpContextAccessor.HttpContext!.Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);
        }
        private async Task SetRefreshTokenForUser((string Token, DateTime CreatedAt, DateTime Expires) refreshToken, ApplicationUser user)
        {
            user.RefreshToken = refreshToken.Token;
            user.TokenExpires = refreshToken.Expires;
            user.TokenCreated = refreshToken.CreatedAt;
            await _repository.SaveChanges();
        }
    }
}
