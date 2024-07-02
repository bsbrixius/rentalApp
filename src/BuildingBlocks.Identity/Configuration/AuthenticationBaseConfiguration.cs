using Authentication.API.Application.Data.Repositories;
using Authentication.API.Infraestructure;
using BuildingBlocks.Identity.Repositories;
using BuildingBlocks.Identity.Services;
using BuildingBlocks.Security.Domain;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace BuildingBlocks.Identity.Configuration
{
    public static class AuthenticationBaseConfiguration
    {
        public static IServiceCollection AddIdentityBase<TUser>(this IServiceCollection services)
            where TUser : UserBase, new()
        {
            services.AddHttpContextAccessor();
            services.AddDbContext<AuthenticationBaseContext<TUser>>();
            services.TryAddScoped<IUserRepository<TUser>, UserRepository<TUser>>();
            services.TryAddScoped<IUserClaimRepository<TUser>, UserClaimRepository<TUser>>();
            services.TryAddScoped<IUserService<TUser>, UserService<TUser>>();
            services.TryAddScoped<ILoginService<TUser>, LoginService<TUser>>();
            services.TryAddScoped<IRoleRepository, RoleRepository<TUser>>();
            services.TryAddScoped<IIdentityService, IdentityService>();

            return services;
        }

        public static IServiceCollection AddIdentityBase<TUser, TDbContext>(this IServiceCollection services)
            where TUser : UserBase, new()
            where TDbContext : AuthenticationBaseContext<TUser>
        {
            services.AddHttpContextAccessor();
            services.TryAddSingleton<AuthenticationBaseContext<TUser>, TDbContext>();
            services.AddDbContext<TDbContext>();
            services.TryAddScoped<IUserRepository<TUser>, UserRepository<TUser>>();
            services.TryAddScoped<IUserClaimRepository<TUser>, UserClaimRepository<TUser>>();
            services.TryAddScoped<IUserService<TUser>, UserService<TUser>>();
            services.TryAddScoped<ILoginService<TUser>, LoginService<TUser>>();
            services.TryAddScoped<IRoleRepository, RoleRepository<TUser>>();
            services.TryAddScoped<IIdentityService, IdentityService>();
            return services;
        }


    }
}
