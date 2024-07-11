using BuildingBlocks.Security.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace BuildingBlocks.API.Core.Security
{
    public class PoliciesConfiguration
    {
        public static void ConfigureAuthorization(AuthorizationOptions options)
        {

            options.AddPolicy(Policies.Roles.Admin.Read, builder => builder.RequireAuthenticatedUser().RequireRole(SystemRoles.Admin).RequireClaim(Claims.Admin.ReadAccess, ClaimValues.Read));
            options.AddPolicy(Policies.Roles.Admin.Write, builder => builder.RequireAuthenticatedUser().RequireRole(SystemRoles.Admin).RequireClaim(Claims.Admin.WriteAccess, ClaimValues.Write));
            options.AddPolicy(Policies.Roles.Admin.Delete, builder => builder.RequireAuthenticatedUser().RequireRole(SystemRoles.Admin).RequireClaim(Claims.Admin.DeleteAccess, ClaimValues.Delete));

            options.AddPolicy(Policies.Roles.Renter.Read, builder => builder.RequireAuthenticatedUser().RequireRole(SystemRoles.Renter).RequireClaim(Claims.Renter.ReadAccess, ClaimValues.Read));
            options.AddPolicy(Policies.Roles.Renter.Write, builder => builder.RequireAuthenticatedUser().RequireRole(SystemRoles.Renter).RequireClaim(Claims.Renter.WriteAccess, ClaimValues.Write));
        }
    }
}
