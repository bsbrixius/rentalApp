using BuildingBlocks.Infrastructure.Data;
using Core.Infrastructure;

namespace Core.API.Domain.Repositories
{
    public interface IMotorcycleRepository : IRepository<Motorcycle>
    {
    }
    public class MotorcycleRepository : Repository<Motorcycle, CoreContext>, IMotorcycleRepository
    {
        public MotorcycleRepository(CoreContext context) : base(context)
        {
        }
    }
}
