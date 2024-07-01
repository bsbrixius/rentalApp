﻿using Authentication.API.Infraestructure;
using BuildingBlocks.Domain.Repositories;
using BuildingBlocks.Security.Domain;
using Microsoft.AspNetCore.Identity;

namespace Authentication.API.Application.Data.Repositories
{
    public interface IUserRepository<TUser> : IRepository<TUser>
        where TUser : UserBase
    {
        Task<bool> UpdatePasswordHashAsync(TUser user, string password);
    }
    public class UserRepository<TUser> : Repository<TUser, AuthenticationBaseContext<TUser>>, IUserRepository<TUser>
        where TUser : UserBase
    {

        public UserRepository(AuthenticationBaseContext<TUser> context) : base(context)
        {
        }

        public async Task<bool> UpdatePasswordHashAsync(TUser user, string password)
        {
            var passwordHasher = new PasswordHasher<TUser>();
            var hash = passwordHasher.HashPassword(user, password);
            user.PasswordHash = hash;
            SetNewSecurityStamp(user);
            return await CommitAsync();
        }

        private void SetNewSecurityStamp(TUser user)
        {
            user.SecurityStamp = Guid.NewGuid().ToString();
        }
    }
}
