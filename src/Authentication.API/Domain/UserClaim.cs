using BuildingBlocks.Domain;

namespace Authentication.API.Domain
{
    public class UserClaim : Entity
    {
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }
}
