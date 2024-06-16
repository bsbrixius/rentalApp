using BuildingBlocks.Domain;

namespace Authentication.API.Domain
{
    public class UserLogin : Entity
    {
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }
        public string ProviderDisplayName { get; set; }
    }
}
