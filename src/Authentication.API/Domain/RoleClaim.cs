using BuildingBlocks.Domain;

namespace Authentication.API.Domain
{
    public class RoleClaim : Entity
    {
        public string RoleId { get; set; }
        public virtual Role Role { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }
}
