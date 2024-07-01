using BuildingBlocks.Security.Domain;

namespace Authentication.API.Domain
{
    public class User : UserBase
    {

        public User() : base()
        {
        }
        public User(string email, params Role[] roles) : this()
        {
            Email = email;
            Roles = roles.ToList();
        }

        public User(string email, string? fullName, DateOnly? birthday, bool active = true) : this()
        {
            Email = email.ToLower();
            FullName = fullName;
            Birthday = birthday;
            Active = active;
        }

        public string? FullName { get; set; }
        public DateOnly? Birthday { get; set; }
    }
}
