using Authentication.API.Application.Data.Repositories;
using BuildingBlocks.Identity.Services;
using BuildingBlocks.Infrastructure.Exceptions;
using MediatR;

namespace Authentication.Application.Commands.User.RegisterUser
{
    public sealed class RegisterUserCommand : IRequest
    {
        public string Email { get; set; }
        public string? FullName { get; set; }
        public string Password { get; set; }
        public DateOnly? Birthday { get; set; }

        internal sealed class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand>
        {
            private readonly IUserService<Domain.Aggregates.User> _userService;
            private readonly IUserRepository<Domain.Aggregates.User> _userRepository;

            public RegisterUserCommandHandler(
                IUserService<Domain.Aggregates.User> userService,
                IUserRepository<Domain.Aggregates.User> userRepository)
            {
                _userService = userService;
                _userRepository = userRepository;
            }

            public async Task Handle(RegisterUserCommand request, CancellationToken cancellationToken)
            {
                var user = await _userService.GetByEmailAsync(request.Email);
                if (user == null)
                    throw new DomainException($"User not found with Email: ${request.Email}");
                if (user.Active)
                    throw new DomainException($"Is already activated with Email: ${request.Email}");

                var success = await _userService.AddPasswordAsync(user, request.Password);
                if (!success)
                    throw new DomainException("Error adding password");
                else
                {
                    user.UpdateUser(request.FullName, request.Birthday);
                    user.ActivateUser();
                    _userRepository.Update(user);
                    await _userRepository.CommitAsync();
                }

            }
        }

    }
}
