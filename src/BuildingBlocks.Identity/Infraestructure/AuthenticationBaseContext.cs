using Authentication.API.Domain;
using Authentication.API.Infraestructure.EntityConfiguration;
using BuildingBlocks.Infrastructure.Base;
using BuildingBlocks.Security.Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Authentication.API.Infraestructure
{
    public class AuthenticationBaseContext<TUser> : BaseDb
        where TUser : UserBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticationBaseContext(
            IMediator mediator,
            ILogger<AuthenticationBaseContext<TUser>> logger,
            IHttpContextAccessor httpContextAccessor,
            DbContextOptions options
            ) : base(mediator, logger, options)
        {
            ArgumentNullException.ThrowIfNull(httpContextAccessor);
            _httpContextAccessor = httpContextAccessor;
        }

        public DbSet<TUser> Users { get; set; }
        public DbSet<UserClaim> UserClaims { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RoleClaim> RoleClaims { get; set; }
        public DbSet<UserToken> UserTokens { get; set; }
        public DbSet<UserLogin> UserLogins { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<UserBase>()
                .ToTable(typeof(TUser).Name);

            builder.Entity<TUser>()
                .ToTable(typeof(TUser).Name)
                .HasBaseType<UserBase>();

            builder.ApplyConfiguration(new RoleConfiguration());
            builder.ApplyConfiguration(new UserBaseConfiguration());
            builder.ApplyConfiguration(new UserRoleConfiguration());
            base.OnModelCreating(builder);
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
                    ((AuditableEntity)entityEntry.Entity).CreatedBy = _httpContextAccessor?.HttpContext?.User?.Identity?.Name ?? Assembly.GetCallingAssembly().FullName;
                }
                else
                {
                    Entry((AuditableEntity)entityEntry.Entity).Property(p => p.CreatedAt).IsModified = false;
                    Entry((AuditableEntity)entityEntry.Entity).Property(p => p.CreatedBy).IsModified = false;
                }

                ((AuditableEntity)entityEntry.Entity).UpdatedAt = DateTime.UtcNow;
                ((AuditableEntity)entityEntry.Entity).UpdatedBy = _httpContextAccessor?.HttpContext?.User?.Identity?.Name ?? Assembly.GetCallingAssembly().FullName;
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
