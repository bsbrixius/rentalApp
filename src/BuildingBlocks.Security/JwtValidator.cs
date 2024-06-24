using BuildingBlocks.Security.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BuildingBlocks.Security
{
    public class JwtValidator
    {
        private readonly JwtSettings _jwtSettings;
        public JwtValidator(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }
        public async Task<TokenValidationResult> ValidateTokenAsync(string token)
        {
            var handler = new JsonWebTokenHandler();

            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            var result = await handler.ValidateTokenAsync(token, new TokenValidationParameters()
            {
                ValidIssuer = _jwtSettings.Issuer,
                ValidAudience = _jwtSettings.Audience,
                RequireSignedTokens = false,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            });

            return result;
        }
    }
}
