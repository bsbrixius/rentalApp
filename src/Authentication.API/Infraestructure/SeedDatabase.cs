﻿using Authentication.API.Domain;
using BuildingBlocks.Security.Authorization;
using BuildingBlocks.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Authentication.API.Infraestructure
{
    public static class SeedDatabase
    {
        public static async Task TrySeedDatabaseAsync(this AuthenticationContext context)
        {
            await SeedRoles(context);
            await SeedUsers(context);

            await context.SaveChangesAsync();
        }

        private static async Task SeedUsers(AuthenticationContext context)
        {
            var adminUser = new User("admin-first-name", "admin-last-name", DateTime.UtcNow.ToDateOnly(), "admin@rentalapp.com");
            if (!context.Users.Any(x => x.Email == adminUser.Email))
            {
                adminUser.PasswordHash = new PasswordHasher<User>().HashPassword(adminUser, "admin123");
                var adminRole = await context.Roles.FirstOrDefaultAsync(x => x.Name == SystemRoles.Admin);
                adminUser.Roles.Add(adminRole);
                context.Users.Add(adminUser);
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedRoles(AuthenticationContext context)
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
                    new RoleClaim(Claims.CustomerService.DeleteAccess, ClaimValues.Delete)
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
