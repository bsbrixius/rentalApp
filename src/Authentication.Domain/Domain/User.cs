using Microsoft.AspNetCore.Identity;

namespace Authentication.Domain.Domain
{
    public class User : IdentityUser
    {
        public User()
        {
            Id = Guid.NewGuid().ToString();
        }

        public User(string username, string firstName, string lastName, DateOnly birthday, string email)
        {
            Id = Guid.NewGuid().ToString();
            UserName = username;
            NormalizedUserName = username.ToLower();
            Email = email;
            NormalizedEmail = email.ToLower();
            FirstName = firstName;
            LastName = lastName;
            Birthday = birthday;
            SecurityStamp = Guid.NewGuid().ToString();
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly? Birthday { get; set; }
    }
}
