using Authentication.API.Application.DTO.User;
using Microsoft.AspNetCore.Identity;

namespace Authentication.API.Application.Queries.User
{
    public interface IUserQueries
    {
        Task<UserDTO> GetByIdAsync(string id);
    }
    public class UserQueries : IUserQueries
    {
        private readonly UserManager<Domain.User> _userManager;

        public UserQueries(UserManager<Domain.User> userManager)
        {
            _userManager = userManager;
        }
        public async Task<UserDTO> GetByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null) return null;
            return UserDTO.From(user);

        }
    }
}
