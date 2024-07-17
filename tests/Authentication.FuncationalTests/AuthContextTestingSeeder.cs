using Authentication.API.Domain;
using Authentication.Domain;
using Authentication.Domain.Aggregates;
using BuildingBlocks.Security.Authorization;
using BuildingBlocks.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Testing.Base.Helpers;

namespace Authentication.FuncationalTests
{
    public class AuthContextTestingSeeder : BaseDatabaseSeederFixture
    {
        public AuthContextTestingSeeder() : base()
        {

        }

        public override void TrySeedDatabase(IHost host)
        {
            base.TrySeedDatabase(host);
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        public async Task TrySeedDevelopmentDatabaseAsync(this AuthenticationContext context)
        {
            await SeedRoles(context);
            await SeedUsers(context);

            await context.SaveChangesAsync();
        }

        private async Task SeedUsers(AuthenticationContext context)
        {
            var adminUser = new User("admin@rentalapp.com", "admin-fullname", DateTime.UtcNow.ToDateOnly());
            if (!context.Users.Any(x => x.Email == adminUser.Email))
            {
                adminUser.PasswordHash = new PasswordHasher<User>().HashPassword(adminUser, "admin123");
                var adminRole = await context.Roles.FirstOrDefaultAsync(x => x.Name == SystemRoles.Admin);
                adminUser.Roles.Add(adminRole);
                context.Users.Add(adminUser);
            }

            var renterUser = new User("renter@outlook.com", "renter-fullname", DateTime.UtcNow.ToDateOnly());
            if (!context.Users.Any(x => x.Email == renterUser.Email))
            {
                renterUser.PasswordHash = new PasswordHasher<User>().HashPassword(renterUser, "renter123");
                var renterRole = await context.Roles.FirstOrDefaultAsync(x => x.Name == SystemRoles.Renter);
                renterUser.Roles.Add(renterRole);
                context.Users.Add(renterUser);
            }
        }

        private async Task SeedRoles(AuthenticationContext context)
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
