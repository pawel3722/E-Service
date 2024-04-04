using Duende.IdentityServer.EntityFramework.Options;
using E_Service.Models;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace E_Service.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options, IOptions<OperationalStoreOptions> operationalStoreOptions)
            : base(options, operationalStoreOptions)
        {

        }

        public DbSet<Message> Messages { get; set; }
        public DbSet<Order> Orders{ get; set; }
        public DbSet<Review> Reviews{ get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<ServiceType> ServiceTypes { get; set; }
        public DbSet<Part> Parts { get; set; }
        public DbSet<Model> Models { get; set; }


    }
}