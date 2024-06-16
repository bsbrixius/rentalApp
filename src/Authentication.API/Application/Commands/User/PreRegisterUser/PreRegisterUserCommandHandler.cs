using Authentication.API.Application.Data.Repositories;
using Authentication.API.Domain;
using MediatR;

namespace Authentication.API.Application.Commands.User.PreRegisterUser
{
    public class PreRegisterUserCommandHandler : IRequestHandler<PreRegisterUserCommand, Guid>
    {
        private readonly IUserRepository _userRepository;

        public PreRegisterUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<Guid> Handle(PreRegisterUserCommand request, CancellationToken cancellationToken)
        {
            var newUser = new Domain.User
            {
                Email = request.Email,
                Roles = new List<Role> { new Role(request.Role) }
            };

            var result = await _userRepository.AddAsync(newUser);

            return result;
        }
    }
}
