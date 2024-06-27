using BuildingBlocks.Security.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace BuildingBlocks.API.Core.Security
{
    public class PoliciesConfiguration
    {
        public static void ConfigureAuthorization(AuthorizationOptions options)
        {

            options.AddPolicy(Policies.Roles.Admin.Read, builder => builder.RequireAuthenticatedUser().RequireRole(SystemRoles.Admin).RequireClaim(Claims.ReadAccess, ClaimValues.Read));
            options.AddPolicy(Policies.Roles.Admin.Write, builder => builder.RequireAuthenticatedUser().RequireRole(SystemRoles.Admin).RequireClaim(Claims.WriteAccess, ClaimValues.Write));
            options.AddPolicy(Policies.Roles.Admin.Delete, builder => builder.RequireAuthenticatedUser().RequireRole(SystemRoles.Admin).RequireClaim(Claims.DeleteAccess, ClaimValues.Delete));

        }
    }
}
