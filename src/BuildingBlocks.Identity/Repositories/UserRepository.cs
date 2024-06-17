using Authentication.API.Infraestructure;
using BuildingBlocks.Infrastructure.Data;
using BuildingBlocks.Security.Domain;
using Microsoft.AspNetCore.Identity;

namespace Authentication.API.Application.Data.Repositories
{
    public interface IUserRepository<TUser> : IRepository<TUser>
        where TUser : UserBase
    {
        Task<bool> UpdatePasswordHashAsync(UserBase user, string password);
    }
    public class UserRepository<TUser> : Repository<TUser, AuthenticationBaseContext<TUser>>, IUserRepository<TUser>
        where TUser : UserBase
    {

        public UserRepository(AuthenticationBaseContext<TUser> context) : base(context)
        {
        }


        public async Task<bool> UpdatePasswordHashAsync(UserBase user, string password)
        {
            var passwordHasher = new PasswordHasher<UserBase>();
            var hash = passwordHasher.HashPassword(user, password);
            user.PasswordHash = hash;
            SetNewSecurityStamp(user);
            return await CommitAsync();
        }

        private void SetNewSecurityStamp(UserBase user)
        {
            user.SecurityStamp = Guid.NewGuid().ToString();
        }
    }
}
