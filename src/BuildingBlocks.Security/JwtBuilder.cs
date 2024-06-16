using BuildingBlocks.Security.Data;
using BuildingBlocks.Security.Domain;
using BuildingBlocks.Security.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace BuildingBlocks.Security
{
    public class JwtBuilder
    {
        private readonly UserManager<UserBase> _userManager;
        private readonly JwtSettings _jwtSettings;

        public JwtBuilder(UserManager<UserBase> userManager, IOptions<JwtSettings> jwtSettings)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<JwtToken> GetAccessAsync(string username)
        {
            var user = _userManager.Users.Where(x => x.Email == username.ToLower()).FirstOrDefault();
            if (user == null) throw new Exception($"User with username {username} not found.");
            var userRoles = await _userManager.GetRolesAsync(user);
            return new JwtToken()
            {
                User = new JwtToken.UserData
                {
                    Id = user.Id.ToString(),
                    Email = user.Email,
                    Roles = userRoles.Select(x => x).ToList()
                },
                AcessToken = await GenerateAccessTokenAsync(user, userRoles),
                RefreshToken = await GenerateRefreshTokenAsync(user, user.Email),
                Expires = DateTime.UtcNow.AddHours(_jwtSettings.Expiration)
            };
        }

        private async Task<string> GenerateAccessTokenAsync(UserBase user, IList<string> userRoles)
        {
            var claims = new ClaimsIdentity();
            claims.AddClaims(await _userManager.GetClaimsAsync(user));
            claims.AddClaims(userRoles.Select(s => new Claim("role", s)));
            claims.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()));
            claims.AddClaim(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            claims.AddClaim(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.AddClaim(new Claim(JwtRegisteredClaimNames.Nbf, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString()));
            claims.AddClaim(new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString(), ClaimValueTypes.Integer64));

            var tokenHandler = new JsonWebTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                Subject = claims,
                Expires = DateTime.UtcNow.AddHours(_jwtSettings.Expiration),
                NotBefore = DateTime.UtcNow,
                IssuedAt = DateTime.UtcNow,
                TokenType = "at+jwt",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return token;
        }

        private async Task<string> GenerateRefreshTokenAsync(UserBase user, string email)
        {

            var jti = Guid.NewGuid().ToString();
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
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });


            await UpdateLastGeneratedClaimAsync(user, jti);

            return token;
        }

        private async Task UpdateLastGeneratedClaimAsync(UserBase user, string jti)
        {
            var claims = await _userManager.GetClaimsAsync(user);

            var newLastRefreshTokenClaim = new Claim("LastRefreshToken", jti);

            var lastRefreshTokenClaim = claims.FirstOrDefault(f => f.Type == "LastRefreshToken");

            if (lastRefreshTokenClaim != null)
                await _userManager.ReplaceClaimAsync(user, lastRefreshTokenClaim, newLastRefreshTokenClaim);
            else
                await _userManager.AddClaimAsync(user, newLastRefreshTokenClaim);
        }
    }
}
