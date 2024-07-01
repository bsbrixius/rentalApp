using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace BuildingBlocks.Identity.Services
{
    public interface IIdentityService
    {
        bool IsAutenticated();
        string GetUserName();
        string GetUserEmail();
        Guid GetUserId();
        string GetClaim(string name);
        List<string> GetUserRoles();
    }
    public class IdentityService : IIdentityService
    {
        private readonly IHttpContextAccessor _accessor;

        public IdentityService(IHttpContextAccessor accessor)
        {
            ArgumentNullException.ThrowIfNull(accessor, nameof(accessor));
            _accessor = accessor;
        }
        public string GetClaim(string name)
        {
            if (_accessor.HttpContext is null) return string.Empty;
            return _accessor.HttpContext.User.FindFirstValue(name) ?? string.Empty;
        }

        public string GetUserEmail()
        {
            if (_accessor.HttpContext is null) return string.Empty;
            return _accessor.HttpContext.User.FindFirstValue(ClaimTypes.Email) ?? string.Empty;
        }

        public Guid GetUserId()
        {
            if (_accessor.HttpContext is null) return default;
            if (Guid.TryParse(_accessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
            {
                return userId;
            }
            return default;
        }

        public string GetUserName()
        {
            if (_accessor.HttpContext is null) return string.Empty;
            return _accessor.HttpContext?.User.Identity?.Name ?? string.Empty;
        }

        public List<string> GetUserRoles()
        {
            if (_accessor.HttpContext is null) return new List<string>();
            var claims = _accessor.HttpContext.User.FindAll(ClaimTypes.Role);
            return claims.Select(x => x.Value).ToList();
        }

        public bool IsAutenticated()
        {
            return _accessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;
        }
    }
}
