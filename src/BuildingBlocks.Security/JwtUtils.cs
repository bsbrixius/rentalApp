using BuildingBlocks.Security.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace BuildingBlocks.Security
{
    public class JwtUtils<TIdentityUser, TIdentityRole, TKey>
        where TIdentityUser : IdentityUser<TKey>
        where TIdentityRole : IdentityRole<TKey>
        where TKey : IEquatable<TKey>
    {
        private readonly UserManager<TIdentityUser> _userManager;
        private readonly RoleManager<TIdentityRole> _roleManager;
        private readonly JwtSettings _jwtSettings;

        public JwtUtils(UserManager<TIdentityUser> userManager, RoleManager<TIdentityRole> roleManager, IOptions<JwtSettings> jwtSettings)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<JwtToken> GetAccessTokenAndRefreshTokenAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) throw new Exception($"User with email {email} not found.");
            var userRoles = await _userManager.GetRolesAsync(user);
            return new JwtToken()
            {
                User = new JwtToken.UserData
                {
                    Id = user.Id.ToString(),
                    Email = user.Email,
                    Roles = userRoles.Select(x => x).ToList()
                },
                AcessToken = await GenerateAccessToken(user, userRoles),
                RefreshToken = await GenerateRefreshToken(user),
                Expires = DateTime.UtcNow.AddHours(_jwtSettings.Expiration)
            };
        }

        private async Task<string> GenerateRefreshToken(TIdentityUser user)
        {
            throw new NotImplementedException();
        }

        private async Task<string> GenerateAccessToken(TIdentityUser user, IList<string> userRoles)
        {
            throw new NotImplementedException();
        }
    }
}
