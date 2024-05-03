using EService.Data;
using EService.Models;
using EService.Repositories;
using EService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Security.Cryptography;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard authorization header using the Bearer scheme (\"bearer {token}\").",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value!)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IServiceTypeRepository, ServiceTypeRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IServiceTypeService, ServiceTypeService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    var roles = new[] { "Admin", "Client", "Serviceman", "Seller", "Manager" };

    foreach (var role in roles)
    {
        if (!await context.Roles.AnyAsync(r => r.Name == role))
            await context.AddAsync(new Role() { Name = role });
    }
    await context.SaveChangesAsync();

    string email = builder.Configuration.GetSection("Identity:Email").Value!;           //admin@admin.com;
    string password = builder.Configuration.GetSection("Identity:Password").Value!;     //Admin_123;
    CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

    if (!await context.Users.AnyAsync(u => u.Email == email))
    {
        var user = new ApplicationUser()
        {
            Name = "Admin",
            Surname = "Admin",
            Email = email,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt
        };

        var adminRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == "Admin");
        user.Roles.Add(adminRole!);
        await context.AddAsync(user);
        await context.SaveChangesAsync();
    }
}

app.Run();

void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
{
    using (var hmac = new HMACSHA512())
    {
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
    }
}