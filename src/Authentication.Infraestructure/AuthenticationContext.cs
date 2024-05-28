using Authentication.Domain.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Authentication.Infraestructure
{
    public class AuthenticationContext : IdentityDbContext<User, Role, Guid>
    {
        public IConfiguration Configuration { get; }
        const string _schema = "identity";
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
            builder.HasDefaultSchema(_schema);
            builder.Entity<UserClaim>().ToTable("UserClaims");
            builder.Entity<RoleClaim>().ToTable("RoleClaims");
            builder.Entity<UserToken>().ToTable("UserTokens");
            builder.Entity<UserLogin>().ToTable("UserLogins");
            builder.Entity<User>().ToTable("Users");
        }
    }
}
