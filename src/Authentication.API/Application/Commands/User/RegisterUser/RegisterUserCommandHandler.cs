using Authentication.API.Application.Data.Repositories;
using Authentication.API.Domain.Exceptions;
using MediatR;

namespace Authentication.API.Application.Commands.User.RegisterUser
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand>
    {
        private readonly IUserRepository _userRepository;

        public RegisterUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.FindByEmailAsync(request.Email);
            if (user != null)
                throw new UserDomainException($"User already exists with Email: ${request.Email}");

            //if (!await _userManager.HasPasswordAsync(user))
            //{
            var success = await _userRepository.AddPasswordAsync(user, request.Password);

            //if (success)
            //    throw new DomainException("Register password error: " + string.Join(';', success.Errors));

            //var jwtBuilder = new JwtBuilder<User, IdentityRole>(_userManager, _roleManager, _appJwtSettings, register.Email);
            //return Ok(await jwtBuilder.GenerateAccessAndRefreshToken());
            throw new NotImplementedException();
            //}
        }
    }
}
