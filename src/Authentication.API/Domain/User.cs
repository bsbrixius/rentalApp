using BuildingBlocks.Security.Domain;

namespace Authentication.API.Domain
{
    public class User : UserBase
    {
        public User()
        {
            Id = Guid.NewGuid();
            Roles = new();
        }

        public User(string firstName, string lastName, DateOnly birthday, string email, bool active = true)
        {
            Id = Guid.NewGuid();
            Email = email.ToLower();
            FirstName = firstName;
            LastName = lastName;
            Birthday = birthday;
            SecurityStamp = Guid.NewGuid().ToString();
            Active = active;
            Roles = new();
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly? Birthday { get; set; }
        public bool Active { get; set; }
        public virtual List<Role> Roles { get; set; }
    }
}
