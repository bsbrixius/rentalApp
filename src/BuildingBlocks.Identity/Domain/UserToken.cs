using BuildingBlocks.Domain.Base;
using BuildingBlocks.Security.Domain;

namespace Authentication.API.Domain
{
    public class UserToken : Entity
    {
        public virtual UserBase User { get; set; }
        public virtual string LoginProvider { get; set; } = default!;
        public virtual string Name { get; set; } = default!;
        public virtual string? Value { get; set; }
    }
}
