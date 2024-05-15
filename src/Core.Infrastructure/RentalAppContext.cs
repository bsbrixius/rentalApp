using Core.Domain.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Core.Infrastructure
{
    public class RentalAppContext : DbContext
    {
        protected IConfiguration Configuration { get; }
        public RentalAppContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"));
        }

        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Motorcycle> Motorcycles { get; set; }

    }
}
