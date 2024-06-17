using Authentication.API.Application.Data.Repositories;
using Authentication.API.Domain.Expections;
using BuildingBlocks.Identity.Repositories;
using BuildingBlocks.Security.Domain;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BuildingBlocks.Identity.Services
{
    public interface IUserService<TUser>
        where TUser : UserBase
    {
        Task<Guid> RegisterUserAsync(TUser user);
        Task<TUser?> FindByEmailAsync(string email);
        Task<bool> AddPasswordAsync(TUser user, string password);
        Task<bool> UpdatePassword(TUser user, string hash);

        Task<bool> ReplaceClaimAsync(TUser user, Claim claim, Claim newClaim);
        Task<bool> AddClaimAsync(TUser user, Claim newClaim);
    }
    public class UserService<TUser> : IUserService<TUser>
        where TUser : UserBase
    {
        private readonly IUserRepository<TUser> _userRepository;
        private readonly IUserClaimRepository<TUser> _userClaimRepository;

        public UserService(
            IUserRepository<TUser> userRepository,
            IUserClaimRepository<TUser> userClaimRepository)
        {
            _userRepository = userRepository;
            _userClaimRepository = userClaimRepository;
        }

        public async Task<bool> AddClaimAsync(TUser user, Claim newClaim)
        {
            return await _userClaimRepository.AddClaimAsync(user, newClaim);
        }

        public async Task<bool> AddPasswordAsync(TUser user, string password)
        {
            var hash = user.PasswordHash;
            if (hash != null)
            {
                throw new DomainException("User already has a password.");
            }
            return await _userRepository.UpdatePasswordHashAsync(user, password);
        }

        public async Task<TUser?> FindByEmailAsync(string email)
        {
            return await _userRepository.QueryNoTrack.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<Guid> RegisterUserAsync(TUser user)
        {
            var id = await _userRepository.AddAsync(user);
            await _userRepository.CommitAsync();
            return id;
        }

        public async Task<bool> ReplaceClaimAsync(TUser user, Claim claim, Claim newClaim)
        {
            return await _userClaimRepository.ReplaceClaimAsync(user, claim, newClaim);
        }

        public async Task<bool> UpdatePassword(TUser user, string password)
        {
            return await _userRepository.UpdatePasswordHashAsync(user, password);
        }
    }

}
