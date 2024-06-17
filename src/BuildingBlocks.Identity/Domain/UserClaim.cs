using BuildingBlocks.Domain;
using BuildingBlocks.Security.Domain;

namespace Authentication.API.Domain
{
    public class UserClaim : Entity
    {
        public Guid UserId { get; set; }
        public virtual UserBase User { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }
}
