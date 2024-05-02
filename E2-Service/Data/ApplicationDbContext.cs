using E2_Service.Models;
using Microsoft.EntityFrameworkCore;

namespace E2_Service.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ApplicationUser> Users => Set<ApplicationUser>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<Message> Messages => Set<Message>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<Review> Reviews => Set<Review>();
        public DbSet<Service> Services => Set<Service>();
        public DbSet<ServiceType> ServiceTypes => Set<ServiceType>();
        public DbSet<Part> Parts => Set<Part>();
        public DbSet<Model> Models => Set<Model>();
    }
}
