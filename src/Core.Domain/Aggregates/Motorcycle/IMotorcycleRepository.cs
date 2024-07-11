using BuildingBlocks.Domain.Repositories;

namespace Core.Domain.Aggregates.Motorcycle
{
    public interface IMotorcycleRepository : IRepository<Motorcycle>
    {
        bool HasAnyRent(Guid id);
        Task<Motorcycle?> GetAvailableAtAsync(DateOnly startAt);
    }
}
