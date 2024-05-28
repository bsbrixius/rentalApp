using Microsoft.AspNetCore.Identity;

namespace Authentication.Domain.Domain
{
    public class User : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly Birthday { get; set; }
    }
}
