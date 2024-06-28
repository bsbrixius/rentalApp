using BuildingBlocks.Domain.Base;

namespace Authentication.API.Domain
{
    public class RoleClaim : Entity
    {
        public RoleClaim(string claimType, string claimValue)
        {
            ClaimType = claimType;
            ClaimValue = claimValue;
        }

        public Guid RoleId { get; set; }
        public virtual Role Role { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }
}
