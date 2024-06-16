using Authentication.API.Domain;
using Authentication.API.Infraestructure.EntityConfiguration;
using BuildingBlocks.Infrastructure.Base;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Authentication.API.Infraestructure
{
    public class AuthenticationContext : BaseDb
    {
        public IConfiguration Configuration { get; }
        const string _schema = "identity";
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticationContext(
            IMediator mediator,
            ILogger<DbContext> logger,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            DbContextOptions<AuthenticationContext> options
            ) : base(mediator, logger, options)
        {
            ArgumentNullException.ThrowIfNull(httpContextAccessor);
            ArgumentNullException.ThrowIfNull(configuration);
            _httpContextAccessor = httpContextAccessor;
            Configuration = configuration;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserClaim> UserClaims { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RoleClaim> RoleClaims { get; set; }
        public DbSet<UserToken> UserTokens { get; set; }
        public DbSet<UserLogin> UserLogins { get; set; }
        //public DbSet<UserRole> UserRoles { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"));
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.HasDefaultSchema(_schema);
            //builder.Entity<User>().ToTable("Users");
            //builder.Entity<UserClaim>().ToTable("UserClaims");
            //builder.Entity<Role>().ToTable("Roles");
            //builder.Entity<RoleClaim>().ToTable("RoleClaims");
            //builder.Entity<UserToken>().ToTable("UserTokens");
            //builder.Entity<UserLogin>().ToTable("UserLogins");
            //builder.Entity<UserRole>().ToTable("UserRoles");

            builder.ApplyConfiguration(new RoleConfiguration());
            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new UserRoleConfiguration());
        }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is AuditableEntity && (
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                if (entityEntry.State == EntityState.Added)
                {
                    ((AuditableEntity)entityEntry.Entity).CreatedAt = DateTime.UtcNow;
                    ((AuditableEntity)entityEntry.Entity).CreatedBy = _httpContextAccessor?.HttpContext?.User?.Identity?.Name ?? typeof(Program).Assembly.FullName;
                }
                else
                {
                    Entry((AuditableEntity)entityEntry.Entity).Property(p => p.CreatedAt).IsModified = false;
                    Entry((AuditableEntity)entityEntry.Entity).Property(p => p.CreatedBy).IsModified = false;
                }

                ((AuditableEntity)entityEntry.Entity).UpdatedAt = DateTime.UtcNow;
                ((AuditableEntity)entityEntry.Entity).UpdatedBy = _httpContextAccessor?.HttpContext?.User?.Identity?.Name ?? typeof(Program).Assembly.FullName;
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
