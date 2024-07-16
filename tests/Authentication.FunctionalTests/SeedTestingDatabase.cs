using Authentication.API.Domain;
using Authentication.Domain;
using Authentication.Domain.Aggregates;
using BuildingBlocks.Security.Authorization;
using BuildingBlocks.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Authentication.FunctionalTests
{
    public static class SeedTestingDatabase
    {
        public static async Task TrySeedTestingDatabaseAsync(this AuthContext context)
        {
            await SeedRoles(context);
            await SeedUsers(context);

            await context.SaveChangesAsync();
        }

        private static async Task SeedUsers(AuthContext context)
        {
            var adminUser = new User("test-admin@rentalapp.com", "test-admin-fullname", DateTime.UtcNow.ToDateOnly());
            if (!context.Users.Any(x => x.Email == adminUser.Email))
            {
                adminUser.PasswordHash = new PasswordHasher<User>().HashPassword(adminUser, "testadmin123");
                var adminRole = await context.Roles.FirstOrDefaultAsync(x => x.Name == SystemRoles.Admin);
                adminUser.Roles.Add(adminRole);
                context.Users.Add(adminUser);
            }

            var renterUser = new User("test-renter@outlook.com", "test-renter-fullname", DateTime.UtcNow.ToDateOnly());
            if (!context.Users.Any(x => x.Email == renterUser.Email))
            {
                renterUser.PasswordHash = new PasswordHasher<User>().HashPassword(renterUser, "test-renter123");
                var renterRole = await context.Roles.FirstOrDefaultAsync(x => x.Name == SystemRoles.Renter);
                renterUser.Roles.Add(renterRole);
                context.Users.Add(renterUser);
            }
        }

        private static async Task SeedRoles(AuthContext context)
        {
            if (!context.Roles.Any(x => x.Name == SystemRoles.Admin))
            {
                var role = new Role(SystemRoles.Admin);
                role.RoleClaims = new List<RoleClaim>
                {
                    new RoleClaim(Claims.Admin.ReadAccess, ClaimValues.Read),
                    new RoleClaim(Claims.Admin.WriteAccess, ClaimValues.Write),
                    new RoleClaim(Claims.Admin.DeleteAccess, ClaimValues.Delete)
                };
                var result = await context.AddAsync(role);
            }

            if (!context.Roles.Any(x => x.Name == SystemRoles.CustomerService))
            {
                var role = new Role(SystemRoles.CustomerService);
                role.RoleClaims = new List<RoleClaim>
                {
                    new RoleClaim(Claims.CustomerService.ReadAccess, ClaimValues.Read),
                    new RoleClaim(Claims.CustomerService.WriteAccess, ClaimValues.Write),
                    //new RoleClaim(Claims.CustomerService.DeleteAccess, ClaimValues.Delete)
                };
                var result = await context.AddAsync(role);
            }

            if (!context.Roles.Any(x => x.Name == SystemRoles.Renter))
            {
                var role = new Role(SystemRoles.Renter);
                role.RoleClaims = new List<RoleClaim>
                {
                    new RoleClaim(Claims.Renter.ReadAccess, ClaimValues.Read),
                    new RoleClaim(Claims.Renter.WriteAccess, ClaimValues.Write),
                };
                var result = await context.AddAsync(role);
            }
            await context.SaveChangesAsync();
        }
    }
}

