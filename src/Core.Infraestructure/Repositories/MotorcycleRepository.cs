using BuildingBlocks.Infrastructure.Data;
using Core.Domain;

namespace Core.Infraestructure.Repositories
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
