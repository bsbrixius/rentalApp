using BuildingBlocks.Security.Authorization;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;
namespace Core.API.FunctionalTests.Helpers
{
    public class TestClaimsProvider
    {
        public IList<Claim> Claims { get; }

        public TestClaimsProvider(IList<Claim> claims)
        {
            Claims = claims;
        }

        public TestClaimsProvider()
        {
            Claims = new List<Claim>();
        }

        public static TestClaimsProvider WithAdminClaims()
        {
            var provider = new TestClaimsProvider();
            provider.Claims.Add(new Claim(ClaimTypes.NameIdentifier, "e9950af4-a5cc-4412-b3a0-babe9d2018b2"));
            provider.Claims.Add(new Claim(JwtRegisteredClaimNames.Sub, "e9950af4-a5cc-4412-b3a0-babe9d2018b2"));
            provider.Claims.Add(new Claim(JwtRegisteredClaimNames.Email, "testing-admin@rental.com"));
            provider.Claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

            provider.Claims.Add(new Claim(ClaimTypes.Role, SystemRoles.Admin));
            provider.Claims.Add(new Claim(BuildingBlocks.Security.Authorization.Claims.Admin.ReadAccess, ClaimValues.Read));
            provider.Claims.Add(new Claim(BuildingBlocks.Security.Authorization.Claims.Admin.WriteAccess, ClaimValues.Write));
            provider.Claims.Add(new Claim(BuildingBlocks.Security.Authorization.Claims.Admin.DeleteAccess, ClaimValues.Delete));

            return provider;
        }

        public static TestClaimsProvider WithUserClaims()
        {
            var provider = new TestClaimsProvider();
            provider.Claims.Add(new Claim(ClaimTypes.NameIdentifier, "66ae875c-7e95-4fb9-819e-4fd666611c2d"));
            provider.Claims.Add(new Claim(JwtRegisteredClaimNames.Sub, "66ae875c-7e95-4fb9-819e-4fd666611c2d"));
            provider.Claims.Add(new Claim(JwtRegisteredClaimNames.Email, "testing-renter@rental.com"));
            provider.Claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

            provider.Claims.Add(new Claim(ClaimTypes.Role, SystemRoles.Renter));
            provider.Claims.Add(new Claim(BuildingBlocks.Security.Authorization.Claims.Renter.ReadAccess, ClaimValues.Read));
            provider.Claims.Add(new Claim(BuildingBlocks.Security.Authorization.Claims.Renter.WriteAccess, ClaimValues.Write));
            //provider.Claims.Add(new Claim(BuildingBlocks.Security.Authorization.Claims.Renter.DeleteAccess, ClaimValues.Delete));

            return provider;
        }
    }
}