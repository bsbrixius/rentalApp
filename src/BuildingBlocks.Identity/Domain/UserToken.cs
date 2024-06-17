using BuildingBlocks.Domain;
using BuildingBlocks.Security.Domain;
using Microsoft.AspNetCore.Identity;

namespace Authentication.API.Domain
{
    public class UserToken : Entity
    {
        public virtual UserBase User { get; set; }
        public virtual string LoginProvider { get; set; } = default!;
        public virtual string Name { get; set; } = default!;
        [ProtectedPersonalData]
        public virtual string? Value { get; set; }
    }
}
