using Authentication.API.Domain;
using Authentication.API.Domain.Expections;
using Authentication.API.Infraestructure;
using BuildingBlocks.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Authentication.API.Application.Data.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> FindByEmailAsync(string email);
        Task<bool> AddPasswordAsync(User user, string password);
        Task<bool> UpdatePassword(User user, string hash);
    }
    public class UserRepository : Repository<User, AuthenticationContext>, IUserRepository
    {
        private readonly AuthenticationContext _context;

        public UserRepository(AuthenticationContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> AddPasswordAsync(User user, string password)
        {
            var hash = user.PasswordHash;
            if (hash != null)
            {
                throw new DomainException("User already has a password.");
            }
            return await UpdatePasswordHashAsync(user, password);
        }

        public Task<User?> FindByEmailAsync(string email)
        {
            return _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> UpdatePassword(User user, string password)
        {
            return await UpdatePasswordHashAsync(user, password);
        }

        private async Task<bool> UpdatePasswordHashAsync(User user, string password)
        {
            var passwordHasher = new PasswordHasher<User>();
            var hash = passwordHasher.HashPassword(user, password);
            user.PasswordHash = hash;
            SetNewSecurityStamp(user);
            return await _context.SaveChangesAsync() > 0;
        }

        private void SetNewSecurityStamp(User user)
        {
            user.SecurityStamp = Guid.NewGuid().ToString();
        }
    }
}
