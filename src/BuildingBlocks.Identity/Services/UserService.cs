using Authentication.API.Application.Data.Repositories;
using Authentication.API.Domain;
using BuildingBlocks.Identity.Repositories;
using BuildingBlocks.Infrastructure.Exceptions;
using BuildingBlocks.Security.Domain;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Z.EntityFramework.Plus;

namespace BuildingBlocks.Identity.Services
{
    public interface IUserService<TUser>
        where TUser : UserBase
    {
        Task<Guid> RegisterUserAsync(TUser user);
        Task<bool> IsEmailAvailableAsync(string email);
        Task<TUser?> GetByEmailAsync(string email);
        Task<TUser?> FindByIdAsync(Guid id);
        Task<bool> AddPasswordAsync(TUser user, string password);
        Task<bool> UpdatePassword(TUser user, string hash);
        Task<bool> ReplaceClaimAsync(TUser user, Claim claim, Claim newClaim);
        Task<bool> AddClaimAsync(TUser user, Claim newClaim);
        Task<List<Claim>> GetClaimsAsync(TUser user);
        Task<List<Role>> GetUserRolesAsync(TUser user);
        Task PreRegisterUserWithRoleAsync(string email, string roleName);
    }
    public class UserService<TUser> : IUserService<TUser>
        where TUser : UserBase, new()
    {
        private readonly IUserRepository<TUser> _userRepository;
        private readonly IUserClaimRepository<TUser> _userClaimRepository;
        private readonly IRoleRepository _roleRepository;

        public UserService(
            IUserRepository<TUser> userRepository,
            IUserClaimRepository<TUser> userClaimRepository,
            IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _userClaimRepository = userClaimRepository;
            _roleRepository = roleRepository;
        }

        public async Task<bool> AddClaimAsync(TUser user, Claim newClaim)
        {
            return await _userClaimRepository.AddClaimAsync(user, newClaim);
        }
        public async Task<List<Claim>> GetClaimsAsync(TUser user)
        {
            return await _userClaimRepository.QueryNoTrack.Where(x => x.UserId == user.Id).Select(x => new Claim(x.ClaimType, x.ClaimValue)).ToListAsync();
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

        public async Task<TUser?> GetByEmailAsync(string email)
        {
            return await _userRepository.GetByEmailAsync(email);
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

        public async Task<TUser?> FindByIdAsync(Guid id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<List<Role>> GetUserRolesAsync(TUser user)
        {
            return await _userRepository.QueryNoTrack
                .Include(x => x.Roles)
                .ThenInclude(x => x.RoleClaims)
                .Where(x => x.Id == user.Id)
                .SelectMany(x => x.Roles)
                .ToListAsync();
        }

        public async Task<bool> IsEmailAvailableAsync(string email)
        {
            return !(await _userRepository.QueryNoTrack.AnyAsync(x => x.Email == email));
        }

        public async Task PreRegisterUserWithRoleAsync(string email, string roleName)
        {
            var role = await _roleRepository.GetByNameAsync(roleName);
            if (role == null)
            {
                throw new NotFoundException($"Role with name {roleName} not found.");
            }
            var user = new TUser();
            user.Email = email;
            user.Roles = new List<Role> { role };

            await _userRepository.AddAsync(user);
            await _userRepository.CommitAsync();
        }
    }

}
