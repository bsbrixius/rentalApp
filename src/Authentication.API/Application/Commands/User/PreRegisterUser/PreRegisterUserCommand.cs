using MediatR;

namespace Authentication.API.Application.Commands.User.PreRegisterUser
{
    public class PreRegisterUserCommand : IRequest<string>
    {
        public string Username { get; init; }
        public string Role { get; init; }
    }
}
