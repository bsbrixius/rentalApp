using Authentication.API.Domain;
using Authentication.API.Domain.Utils;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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

            var adminUser = new User("admin", "Admin", "Admin", new DateOnly(), "admin@rentalapp.com");
            if (!context.Users.Any(x => x.Email == adminUser.Email))
            {
                adminUser.PasswordHash = new PasswordHasher<User>().HashPassword(adminUser, "admin123");

                var userStore = new UserStore<User, Role, AuthenticationContext>(context);
                var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

                //var result = await userStore.CreateAsync(adminUser);
                var result = await userManager.CreateAsync(adminUser, "admin123");

                if (result.Succeeded)
                {
                    await userStore.AddToRoleAsync(adminUser, UserRole.Admin.NormalizedName);
                }
            }
        }

        private static async Task SeedRoles(AuthenticationContext context)
        {
            var roleStore = new RoleStore<Role>(context);

            if (!context.Roles.Any(x => x.Name == UserRole.Admin.Name))
            {
                var role = new Role() { Name = UserRole.Admin.Name, NormalizedName = UserRole.Admin.NormalizedName };
                var result = await roleStore.CreateAsync(role);
            }

            if (!context.Roles.Any(x => x.Name == UserRole.CustomerService.Name))
            {
                var role = new Role() { Name = UserRole.CustomerService.Name, NormalizedName = UserRole.CustomerService.NormalizedName };
                var result = await roleStore.CreateAsync(role);
            }

            if (!context.Roles.Any(x => x.Name == UserRole.Driver.Name))
            {
                var role = new Role() { Name = UserRole.Driver.Name, NormalizedName = UserRole.Driver.NormalizedName };
                var result = await roleStore.CreateAsync(role);
            }
        }
    }
}
