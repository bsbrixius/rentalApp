using Authentication.API.Domain;
using Authentication.API.Infraestructure;
using BuildingBlocks.Infrastructure.Data;
using BuildingBlocks.Security.Domain;
using System.Security.Claims;

namespace BuildingBlocks.Identity.Repositories
{
    public interface IUserClaimRepository<TUser> : IRepository<UserClaim>
        where TUser : UserBase
    {
        Task<bool> ReplaceClaimAsync(TUser user, Claim claim, Claim newClaim);
        Task<bool> AddClaimAsync(TUser user, Claim newClaim);
    }
    public class UserClaimRepository<TUser> : Repository<UserClaim, AuthenticationBaseContext<TUser>>, IUserClaimRepository<TUser>
        where TUser : UserBase
    {

        public UserClaimRepository(AuthenticationBaseContext<TUser> context) : base(context)
        {
        }

        public async Task<bool> AddClaimAsync(TUser user, Claim newClaim)
        {
            var claim = new UserClaim
            {
                UserId = user.Id,
                ClaimType = newClaim.Type,
                ClaimValue = newClaim.Value
            };
            await _dbSet.AddAsync(claim);
            return await CommitAsync();
        }

        public async Task<bool> ReplaceClaimAsync(TUser user, Claim claim, Claim newClaim)
        {
            var claims = _dbSet.Where(x => x.UserId == user.Id && x.ClaimType == claim.Type && x.ClaimValue == claim.Value);
            foreach (var c in claims)
            {
                c.ClaimType = newClaim.Type;
                c.ClaimValue = newClaim.Value;
            }
            return await CommitAsync();
        }
    }
}
