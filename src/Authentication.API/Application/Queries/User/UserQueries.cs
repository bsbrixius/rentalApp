using Authentication.API.Application.DTO.User;
using BuildingBlocks.Identity.Services;

namespace Authentication.API.Application.Queries.User
{
    public interface IUserQueries
    {
        Task<UserDTO> GetByIdAsync(Guid id);
    }
    public class UserQueries : IUserQueries
    {
        private readonly IUserService<Domain.User> _userService;

        public UserQueries(IUserService<Domain.User> userService)
        {
            _userService = userService;
        }
        public async Task<UserDTO> GetByIdAsync(Guid id)
        {
            var user = await _userService.FindByIdAsync(id);
            if (user is null) return null;
            return UserDTO.From(user);

        }
    }
}
