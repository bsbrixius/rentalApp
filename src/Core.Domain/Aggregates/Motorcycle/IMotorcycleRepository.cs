using BuildingBlocks.Domain.Repositories;
using Core.Domain.Aggregates.Motorcycle;

namespace Core.Infraestructure.Repositories
{
    public interface IMotorcycleRepository : IRepository<Motorcycle>
    {
    }
}
