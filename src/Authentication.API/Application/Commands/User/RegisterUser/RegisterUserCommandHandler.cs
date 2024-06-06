using Authentication.API.Domain.Exceptions;
using Authentication.API.Domain.Expections;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Authentication.API.Application.Commands.User.RegisterUser
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand>
    {
        private readonly UserManager<Domain.User> _userManager;

        public RegisterUserCommandHandler(UserManager<Domain.User> userManager)
        {
            _userManager = userManager;
        }

        public async Task Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user != null)
                throw new UserDomainException($"User already exists with Email: ${request.Email}");

            if (!await _userManager.HasPasswordAsync(user))
            {
                var result = await _userManager.AddPasswordAsync(user, request.Password);

                if (!result.Succeeded)
                    throw new DomainException("Register password error: " + string.Join(';', result.Errors));

                //var jwtBuilder = new JwtBuilder<User, IdentityRole>(_userManager, _roleManager, _appJwtSettings, register.Email);
                //return Ok(await jwtBuilder.GenerateAccessAndRefreshToken());
                throw new NotImplementedException();
            }
        }
    }
}
