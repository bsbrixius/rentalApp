using BuildingBlocks.Domain.Repositories;
using Core.Domain.Aggregates.Motorcycle;
using Core.Infraestructure.Repositories;

namespace Core.Data.Repositories
{
    public class MotorcycleRepository : Repository<Motorcycle, CoreContext>, IMotorcycleRepository
    {
        public MotorcycleRepository(CoreContext context) : base(context)
        {
        }
    }
}
