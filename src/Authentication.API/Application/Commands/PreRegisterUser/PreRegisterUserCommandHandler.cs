using Authentication.API.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Authentication.API.Application.Commands.PreRegisterUser
{
    public class PreRegisterUserCommandHandler : IRequestHandler<PreRegisterUserCommand, string>
    {
        private readonly UserManager<User> _userManager;

        public PreRegisterUserCommandHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        public async Task<string> Handle(PreRegisterUserCommand request, CancellationToken cancellationToken)
        {
            var newUser = new User
            {
                UserName = request.Username
            };
            var result = await _userManager.CreateAsync(newUser);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, request.Role);
                return newUser.Id;
            }
            //TODO implementar Errors
            return null;
        }
    }
}
