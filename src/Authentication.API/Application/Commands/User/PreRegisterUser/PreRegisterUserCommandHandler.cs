using Authentication.API.Domain;
using BuildingBlocks.Identity.Services;
using MediatR;

namespace Authentication.API.Application.Commands.User.PreRegisterUser
{
    public class PreRegisterUserCommandHandler : IRequestHandler<PreRegisterUserCommand, Guid>
    {
        private readonly IUserService<Domain.User> _userService;

        public PreRegisterUserCommandHandler(IUserService<Domain.User> userService)
        {
            _userService = userService;
        }
        public async Task<Guid> Handle(PreRegisterUserCommand request, CancellationToken cancellationToken)
        {
            var newUser = new Domain.User(request.Email, new Role(request.Role));
            return await _userService.RegisterUserAsync(newUser);
        }
    }
}
