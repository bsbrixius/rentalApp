using Authentication.API.Domain.Exceptions;
using BuildingBlocks.Identity.Services;
using MediatR;

namespace Authentication.API.Application.Commands.User.RegisterUser
{
    public sealed class RegisterUserCommand : IRequest
    {
        public string Email { get; set; }
        public string? FullName { get; set; }
        public string Password { get; set; }
        public DateOnly? BirthDay { get; set; }

        internal sealed class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand>
        {
            private readonly IUserService<Domain.User> _userService;

            public RegisterUserCommandHandler(IUserService<Domain.User> userService)
            {
                _userService = userService;
            }

            public async Task Handle(RegisterUserCommand request, CancellationToken cancellationToken)
            {
                var user = await _userService.FindByEmailAsync(request.Email);
                if (user != null)
                    throw new UserDomainException($"User already exists with Email: ${request.Email}");

                //if (!await _userManager.HasPasswordAsync(user))
                //{
                var success = await _userService.AddPasswordAsync(user, request.Password);

                //if (success)
                //    throw new DomainException("Register password error: " + string.Join(';', success.Errors));

                //var jwtBuilder = new JwtBuilder<User, IdentityRole>(_userManager, _roleManager, _appJwtSettings, register.Email);
                //return Ok(await jwtBuilder.GenerateAccessAndRefreshToken());
                throw new NotImplementedException();
                //}
            }
        }

    }
}
