using BuildingBlocks.Infrastructure.Data;
using Core.API.Domain;
using Core.Infrastructure;

namespace Core.API.Application.Data.Repositories
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
