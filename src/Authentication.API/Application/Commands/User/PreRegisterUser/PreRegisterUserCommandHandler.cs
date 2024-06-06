using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Authentication.API.Application.Commands.User.PreRegisterUser
{
    public class PreRegisterUserCommandHandler : IRequestHandler<PreRegisterUserCommand, string>
    {
        private readonly UserManager<Domain.User> _userManager;

        public PreRegisterUserCommandHandler(UserManager<Domain.User> userManager)
        {
            _userManager = userManager;
        }
        public async Task<string> Handle(PreRegisterUserCommand request, CancellationToken cancellationToken)
        {
            var newUser = new Domain.User
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
