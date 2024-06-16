using BuildingBlocks.Domain;
using Microsoft.AspNetCore.Identity;

namespace Authentication.API.Domain
{
    public class UserToken : Entity
    {
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public virtual string LoginProvider { get; set; } = default!;
        public virtual string Name { get; set; } = default!;
        [ProtectedPersonalData]
        public virtual string? Value { get; set; }
    }
}
