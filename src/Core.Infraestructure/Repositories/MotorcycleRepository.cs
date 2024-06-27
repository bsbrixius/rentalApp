using BuildingBlocks.Domain.Repositories;
using Core.Domain.Aggregates.Motorcycle;

namespace Core.Infraestructure.Repositories
{
    public class MotorcycleRepository : Repository<Motorcycle, CoreContext>, IMotorcycleRepository
    {
        public MotorcycleRepository(CoreContext context) : base(context)
        {
        }
    }
}
