using BuildingBlocks.Domain.Repositories;
using Core.Domain.Aggregates.Motorcycle;

namespace Core.Data.Repositories
{
    public class MotorcycleRepository : Repository<Motorcycle, CoreContext>, IMotorcycleRepository
    {
        public MotorcycleRepository(CoreContext context) : base(context)
        {
        }

        public bool HasAnyRent(Guid id)
        {
            return _dbSet.Any(x => x.Rents.Any());
        }
    }
}
