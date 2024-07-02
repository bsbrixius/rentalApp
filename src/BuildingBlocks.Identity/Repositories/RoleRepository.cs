using Authentication.API.Domain;
using Authentication.API.Infraestructure;
using BuildingBlocks.Domain.Repositories;
using BuildingBlocks.Security.Domain;
using Microsoft.EntityFrameworkCore;

namespace BuildingBlocks.Identity.Repositories
{
    public interface IRoleRepository : IRepository<Role>
    {
        Task<Role?> GetByNameAsync(string roleName);
    }
    public class RoleRepository<TUser> : Repository<Role, AuthenticationBaseContext<TUser>>, IRoleRepository
        where TUser : UserBase
    {
        public RoleRepository(AuthenticationBaseContext<TUser> context) : base(context)
        {
        }

        public async Task<Role?> GetByNameAsync(string roleName)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.Name == roleName);
        }
    }
}
