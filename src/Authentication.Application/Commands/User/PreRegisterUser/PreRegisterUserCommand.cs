using BuildingBlocks.Identity.Services;
using BuildingBlocks.Infrastructure.Exceptions;
using MediatR;

namespace Authentication.Application.Commands.User.PreRegisterUser
{
    public sealed class PreRegisterUserCommand : IRequest
    {
        public string Email { get; init; }
        public string Role { get; init; }

        internal sealed class PreRegisterUserCommandHandler : IRequestHandler<PreRegisterUserCommand>
        {
            private readonly IUserService<Domain.Aggregates.User> _userService;

            public PreRegisterUserCommandHandler(
                IUserService<Domain.Aggregates.User> userService
                )
            {
                _userService = userService;
            }
            public async Task Handle(PreRegisterUserCommand request, CancellationToken cancellationToken)
            {
                var isEmailAvailable = await _userService.IsEmailAvailableAsync(request.Email);
                if (!isEmailAvailable)
                {
                    throw new DomainException($"User already exists with Email: ${request.Email}");
                }
                await _userService.PreRegisterUserWithRoleAsync(request.Email, request.Role);
            }
        }
    }
}
