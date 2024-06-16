using Authentication.API.Domain;
using BuildingBlocks.Security.Authorization;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Authentication.API.Infraestructure
{
    public static class AuthenticationContextSeed
    {
        public static async Task TrySeedDatabaseAsync(this AuthenticationContext context, IServiceProvider serviceProvider)
        {
            await SeedRoles(context);
            await SeedUsers(context, serviceProvider);

            await context.SaveChangesAsync();
        }

        private static async Task SeedUsers(AuthenticationContext context, IServiceProvider serviceProvider)
        {
            var bearerTokenOptions = serviceProvider.GetRequiredService<IOptionsMonitor<BearerTokenOptions>>();

            var adminUser = new User("admin-first-name", "admin-last-name", new DateOnly(), "admin@rentalapp.com");
            if (!context.Users.Any(x => x.Email == adminUser.Email))
            {
                adminUser.PasswordHash = new PasswordHasher<User>().HashPassword(adminUser, "admin123");
                var adminRole = await context.Roles.FirstOrDefaultAsync(x => x.Name == SystemRoles.Admin.Name);
                adminUser.Roles.Add(adminRole);
                context.Users.Add(adminUser);
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedRoles(AuthenticationContext context)
        {
            //var roleStore = new RoleStore<Role>(context);

            if (!context.Roles.Any(x => x.Name == SystemRoles.Admin.Name))
            {
                var role = new Role(SystemRoles.Admin.Name);
                var result = await context.AddAsync(role);
            }

            if (!context.Roles.Any(x => x.Name == SystemRoles.CustomerService.Name))
            {
                var role = new Role(SystemRoles.CustomerService.Name);
                var result = await context.AddAsync(role);
            }

            if (!context.Roles.Any(x => x.Name == SystemRoles.Driver.Name))
            {
                var role = new Role(SystemRoles.Driver.Name);
                var result = await context.AddAsync(role);
            }
            await context.SaveChangesAsync();
        }
    }
}
