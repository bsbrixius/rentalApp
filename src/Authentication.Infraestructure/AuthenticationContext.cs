using Authentication.Domain.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Authentication.Infraestructure
{
    public class AuthenticationContext : IdentityDbContext<User>
    {
        public IConfiguration Configuration { get; }
        public AuthenticationContext(IConfiguration configuration, DbContextOptions<AuthenticationContext> options) : base(options)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"));
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.HasDefaultSchema("identity");
        }
    }
}
