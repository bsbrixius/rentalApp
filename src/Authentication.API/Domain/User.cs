using BuildingBlocks.Security.Domain;

namespace Authentication.API.Domain
{
    public class User : UserBase
    {

        public User()
        {
        }
        public User(string email, params Role[] roles)
        {
            Email = email;
            Roles = roles.ToList();
        }

        public User(string firstName, string lastName, DateOnly birthday, string email, bool active = true)
        {
            Email = email.ToLower();
            FirstName = firstName;
            LastName = lastName;
            Birthday = birthday;
            SecurityStamp = Guid.NewGuid().ToString();
            Active = active;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly? Birthday { get; set; }
    }
}
