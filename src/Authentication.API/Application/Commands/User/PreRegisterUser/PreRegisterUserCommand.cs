using MediatR;

namespace Authentication.API.Application.Commands.User.PreRegisterUser
{
    public class PreRegisterUserCommand : IRequest<Guid>
    {
        public string Email { get; init; }
        public string Role { get; init; }
    }
}
