using Authentication.API.Domain;
using Authentication.API.Infraestructure;
using BuildingBlocks.Domain.Repositories;
using BuildingBlocks.Security.Domain;

namespace BuildingBlocks.Identity.Repositories
{
    public interface IRoleRepository : IRepository<Role>
    {
    }
    public class RoleRepository<TUser> : Repository<Role, AuthenticationBaseContext<TUser>>, IRoleRepository
        where TUser : UserBase
    {
        public RoleRepository(AuthenticationBaseContext<TUser> context) : base(context)
        {
        }
    }
}
