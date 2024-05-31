using MediatR;

namespace Authentication.API.Application.Commands.RegisterUser
{
    public class RegisterUserCommand : IRequest
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public DateOnly? BirthDay { get; set; }

    }
}
