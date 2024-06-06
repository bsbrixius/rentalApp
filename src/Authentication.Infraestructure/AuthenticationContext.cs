using Authentication.Domain.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Authentication.Infraestructure
{
    public class AuthenticationContext : IdentityDbContext<User, Role, string>
    {
        public IConfiguration Configuration { get; }
        const string _schema = "identity";
        public AuthenticationContext(IConfiguration configuration, DbContextOptions<AuthenticationContext> options) : base(options)
        {
            Configuration = configuration;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserClaim> UserClaims { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RoleClaim> RoleClaims { get; set; }
        public DbSet<UserToken> UserTokens { get; set; }
        public DbSet<UserLogin> UserLogins { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"));
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.HasDefaultSchema(_schema);
            builder.Entity<User>().ToTable("Users");
            builder.Entity<UserClaim>().ToTable("UserClaims");
            builder.Entity<Role>().ToTable("Roles");
            builder.Entity<RoleClaim>().ToTable("RoleClaims");
            builder.Entity<UserToken>().ToTable("UserTokens");
            builder.Entity<UserLogin>().ToTable("UserLogins");
        }
    }
}
