using AutoMapper;
using EService.Dtos.ApplicationUserDtos;
using EService.Dtos.AuthDtos;
using EService.Dtos.MessageDtos;
using EService.Dtos.RolesDtos;
using EService.Models;
using EService.Repositories;
using EService.Repositories.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace EService.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public AuthService(IAuthRepository authRepository, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _authRepository = authRepository;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }
        
        public async Task<ReturnApplicationUserDto?> GetUserAsync(int id)
        {
            var user = await _authRepository.GetUserByIdAsync(id);
            var userDto = _mapper.Map<ReturnApplicationUserDto>(user);
            return userDto;
        }
        public async Task<(bool Confirmed, string Response)> RegisterUserAsync(UserRegisterRequestDto request)
        {
            if (await _authRepository.UserExistsAsync(request.Email)) return await Task.FromResult((false, "User with specified email already exists."));
            var role = await _authRepository.GetRoleByNameAsync("Client");
            if (role == null)
            {
                role = new Role() { Name = "Client" };
                await _authRepository.AddRoleAsync(role);
            }
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
            /*role!.Users.Add(newUser);*/
            await _authRepository.AddUserAsync(newUser);
            return await Task.FromResult((true, "User has been succesfully created."));
        }
        public async Task<(bool Confirmed, string Response, TokensResponseDto? Tokens)> LoginUserAsync(UserLoginRequestDto request)
        {
            var user = await _authRepository.GetUserByEmailAsync(request.Email);
            if (user == null) return await Task.FromResult<(bool Confirmed, string Response, TokensResponseDto? Tokens)>((false, "Incorrect email or password.", null));
            if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt)) return await Task.FromResult<(bool, string, TokensResponseDto?)>((false, "Incorrect email or password.", null));
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
            await SetRefreshTokenForUserAsync(refreshToken, user);
            return await Task.FromResult<(bool Confirmed, string Response, TokensResponseDto? Tokens)>((true, $"Welcome, {user.Name}.", tokens));
        }
        public async Task<(bool Confirmed, string Response, TokensResponseDto? Tokens)> RefreshTokenAsync()
        {
            var refreshToken = _httpContextAccessor.HttpContext!.Request.Cookies["refreshToken"];
            var user = await _authRepository.GetUserByRefreshTokenAsync(refreshToken!);
            if (user == null) return await Task.FromResult<(bool Confirmed, string Response, TokensResponseDto? Tokens)>((false, "Invalid refresh token.", null));
            if (user.TokenExpires < DateTime.Now) return await Task.FromResult<(bool Confirmed, string Response, TokensResponseDto? Tokens)>((false, "Token expired.", null));
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
            await SetRefreshTokenForUserAsync(newRefreshToken, user);
            return await Task.FromResult<(bool Confirmed, string Response, TokensResponseDto? Tokens)>((true, $"Welcome {user.Name}.", tokens));
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
        private async Task SetRefreshTokenForUserAsync((string Token, DateTime CreatedAt, DateTime Expires) refreshToken, ApplicationUser user)
        {
            user.RefreshToken = refreshToken.Token;
            user.TokenExpires = refreshToken.Expires;
            user.TokenCreated = refreshToken.CreatedAt;
            await _authRepository.SaveChangesAsync();
        }
    }
}
