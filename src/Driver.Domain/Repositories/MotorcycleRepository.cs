using BuildingBlocks.Infrastructure.Data;
using Core.Infrastructure;
using Driver.Domain;

namespace Driver.Domain.Repositories
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
