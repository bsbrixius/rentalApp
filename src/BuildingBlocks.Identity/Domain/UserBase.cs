using Authentication.API.Domain;
using BuildingBlocks.Domain.Base;

namespace BuildingBlocks.Security.Domain
{
    public class UserBase : Entity
    {
        public UserBase() : base()
        {
            SecurityStamp = Guid.NewGuid().ToString();
        }
        public bool Active { get; set; } = false;
        public string Email { get; set; }

        public UserBase(string email, params Role[] roles) : this()
        {
            Email = email;
            Roles = roles.ToList();
        }
        public UserBase(string email, Role role) : this()
        {
            Email = email;
            Roles = new List<Role>() { role };
        }

        public bool EmailConfirmed { get; set; } = false;
        public string? PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; } = false;
        public int AccessFailedCount { get; set; } = 0;

        /// <summary>
        /// Gets or sets a salted and hashed representation of the password for this user.
        /// </summary>
        public virtual string? PasswordHash { get; set; }

        /// <summary>
        /// A random value that must change whenever a users credentials change (password changed, login removed)
        /// </summary>
        public virtual string? SecurityStamp { get; set; }

        /// <summary>
        /// A random value that must change whenever a user is persisted to the store
        /// </summary>
        public virtual string? ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();
        public virtual List<Role> Roles { get; set; } = new();
        public virtual List<UserClaim> UserClaims { get; set; } = new();
    }
}
