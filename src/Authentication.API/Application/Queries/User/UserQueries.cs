using Authentication.API.Application.Data.Repositories;
using Authentication.API.Application.DTO.User;
using BuildingBlocks.API.Core.Data.Pagination;

namespace Authentication.API.Application.Queries.User
{
    public interface IUserQueries
    {
        Task<UserDTO> GetByIdAsync(Guid id);
        Task<PaginatedResult<UserDTO>> GetPaginated(PaginatedRequest paginatedRequest);
    }
    public class UserQueries : IUserQueries
    {
        private readonly IUserRepository<Domain.User> _userRepository;

        public UserQueries(IUserRepository<Domain.User> userRepository)
        {
            ArgumentNullException.ThrowIfNull(userRepository, nameof(userRepository));
            _userRepository = userRepository;
        }

        public async Task<PaginatedResult<UserDTO>> GetPaginated(PaginatedRequest paginatedRequest)
        {
            var paginatedUsers = await _userRepository.QueryNoTrack.PaginateAsync(paginatedRequest.Page, paginatedRequest.PageSize, UserDTO.From);
            if (paginatedUsers is null) return null;
            return paginatedUsers;
        }
        public async Task<UserDTO> GetByIdAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user is null) return null;
            return UserDTO.From(user);
        }
    }
}
