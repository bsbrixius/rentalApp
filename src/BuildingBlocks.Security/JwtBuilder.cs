using BuildingBlocks.Identity.Services;
using BuildingBlocks.Security.Data;
using BuildingBlocks.Security.Domain;
using BuildingBlocks.Security.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace BuildingBlocks.Security
{
    public class JwtBuilder<TUser>
        where TUser : UserBase

    {
        private readonly JwtSettings _jwtSettings;
        private readonly IUserService<TUser> _userService;

        public JwtBuilder(
            IUserService<TUser> userService,
            IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
            _userService = userService;
        }

        public async Task<JwtToken> CreateAccessAsync(string email)
        {
            ArgumentNullException.ThrowIfNull(email);
            var user = await _userService.FindByEmailAsync(email);
            return new JwtToken()
            {
                User = new JwtToken.UserData
                {
                    Id = user.Id.ToString(),
                    Email = user.Email,
                    Roles = user.Roles.Select(x => x.Name).ToList()
                },
                AcessToken = await GenerateAccessTokenAsync(user),
                RefreshToken = await GenerateRefreshTokenAsync(user, user.Email),
                Expires = DateTime.UtcNow.AddHours(_jwtSettings.Expiration)
            };
        }

        private async Task<string> GenerateAccessTokenAsync(TUser user)
        {
            var claims = new ClaimsIdentity();
            var userClaims = await _userService.GetClaimsAsync(user);
            var roles = await _userService.GetUserRolesAsync(user);

            claims.AddClaims(userClaims);
            claims.AddClaims(roles.Select(x => new Claim("role", x.Name)).ToList());
            claims.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()));
            claims.AddClaim(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            claims.AddClaim(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.AddClaim(new Claim(JwtRegisteredClaimNames.Nbf, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()));
            claims.AddClaim(new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64));

            var tokenHandler = new JsonWebTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
            credentials.Key.KeyId = Guid.NewGuid().ToString();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                Subject = claims,
                Expires = DateTime.UtcNow.AddHours(_jwtSettings.Expiration),
                NotBefore = DateTime.UtcNow,
                IssuedAt = DateTime.UtcNow,
                TokenType = "at+jwt",
                SigningCredentials = credentials
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return token;
        }

        private async Task<string> GenerateRefreshTokenAsync(TUser user, string email)
        {

            var jti = Guid.NewGuid().ToString();
            var userClaims = await _userService.GetClaimsAsync(user);

            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim(JwtRegisteredClaimNames.Email, email));
            claims.AddClaim(new Claim(JwtRegisteredClaimNames.Jti, jti));

            var tokenHandler = new JsonWebTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                Subject = claims,
                Expires = DateTime.UtcNow.AddDays(30),
                NotBefore = DateTime.UtcNow,
                IssuedAt = DateTime.UtcNow,
                TokenType = "rt+jwt",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            });


            await UpdateLastGeneratedClaimAsync(user, jti, userClaims.ToArray());

            return token;
        }

        private async Task UpdateLastGeneratedClaimAsync(TUser user, string jti, params Claim[] userClaims)
        {
            var newLastRefreshTokenClaim = new Claim(CustomClaim.LastRefreshToken, jti);

            var lastRefreshTokenClaim = userClaims.FirstOrDefault(f => f.Type == CustomClaim.LastRefreshToken);

            if (lastRefreshTokenClaim != null)
                await _userService.ReplaceClaimAsync(user, lastRefreshTokenClaim, newLastRefreshTokenClaim);
            else
                await _userService.AddClaimAsync(user, newLastRefreshTokenClaim);
        }
    }
}
