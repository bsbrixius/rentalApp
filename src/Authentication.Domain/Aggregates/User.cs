using Authentication.API.Domain;
using BuildingBlocks.Infrastructure.Exceptions;
using BuildingBlocks.Security.Domain;

namespace Authentication.Domain.Aggregates
{
    public class User : UserBase
    {

        public User() : base()
        {
        }
        public User(string email, params Role[] roles) : this()
        {
            ValidateEmail(email);

            Email = email.ToLower();
            Roles = roles.ToList();
        }

        public User(string email, string? fullName, DateOnly? birthday, bool active = true) : this()
        {
            ValidateEmail(email);

            Email = email.ToLower();
            FullName = fullName;
            Birthday = birthday;
            Active = active;
        }

        public string? FullName { get; set; }
        public DateOnly? Birthday { get; set; }

        public void UpdateUser(string? fullName, DateOnly? birthday)
        {
            FullName = fullName;
            Birthday = birthday;
        }

        public void ActivateUser()
        {
            Active = true;
        }

        private void ValidateEmail(string email)
        {
            if (!Utils.Utils.IsValidEmail(email))
                throw new DomainException("Invalid email format");
        }
    }
}
