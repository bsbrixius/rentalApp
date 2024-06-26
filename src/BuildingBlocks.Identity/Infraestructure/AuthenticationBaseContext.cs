using Authentication.API.Domain;
using Authentication.API.Infraestructure.EntityConfiguration;
using BuildingBlocks.Domain.Base;
using BuildingBlocks.Security.Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Authentication.API.Infraestructure
{
    public class AuthenticationBaseContext<TUser> : BaseDb
        where TUser : UserBase
    {

        public AuthenticationBaseContext(
            IMediator mediator,
            ILogger<DbContext> logger,
            IHttpContextAccessor httpContextAccessor,
            DbContextOptions options
            ) : base(mediator, logger, httpContextAccessor, options)
        {
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
    }
}
