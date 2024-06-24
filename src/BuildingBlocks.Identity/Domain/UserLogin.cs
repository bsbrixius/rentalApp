using BuildingBlocks.Domain;
using BuildingBlocks.Security.Domain;

namespace Authentication.API.Domain
{
    public class UserLogin : Entity
    {
        public virtual UserBase User { get; set; }
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }
        public string ProviderDisplayName { get; set; }
    }
}
