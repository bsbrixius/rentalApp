using Authentication.API.Application.Data.Repositories;
using BuildingBlocks.Security.Domain;
using Microsoft.AspNetCore.Identity;

namespace BuildingBlocks.Identity.Services
{
    public interface ILoginService<TUser>
        where TUser : UserBase
    {
        bool PasswordSignIn(string email, string password);
        bool PasswordSignIn(TUser user, string password);
    }

    public class LoginService<TUser> : ILoginService<TUser>
        where TUser : UserBase
    {
        private readonly IUserRepository<TUser> _userRepository;

        public LoginService(IUserRepository<TUser> userRepository)
        {
            _userRepository = userRepository;
        }

        public bool PasswordSignIn(string email, string password)
        {
            var user = _userRepository.QueryNoTrack.FirstOrDefault(u => u.Email == email);
            if (user == null) return false;
            return CheckPassword(user, password) == PasswordVerificationResult.Success;
        }

        public bool PasswordSignIn(TUser user, string password)
        {
            return CheckPassword(user, password) == PasswordVerificationResult.Success;
        }

        private PasswordVerificationResult CheckPassword(TUser user, string password)
        {
            var passwordHasher = new PasswordHasher<TUser>();
            return passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
        }
    }
}
