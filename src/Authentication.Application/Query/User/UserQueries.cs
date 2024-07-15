using Authentication.API.Application.Data.Repositories;
using Authentication.Application.DTOs.User;
using BuildingBlocks.API.Core.Data.Pagination;

namespace Authentication.Application.Query.User
{
    public interface IUserQueries
    {
        Task<UserDTO?> GetByUserIdAsync(Guid id);
        Task<PaginatedResult<UserDTO>> GetPaginated(PaginatedRequest paginatedRequest);
    }
    public class UserQueries : IUserQueries
    {
        private readonly IUserRepository<Domain.Aggregates.User> _userRepository;

        public UserQueries(IUserRepository<Domain.Aggregates.User> userRepository)
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
        public async Task<UserDTO?> GetByUserIdAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user is null) return null;
            return UserDTO.From(user);
        }
    }
}
