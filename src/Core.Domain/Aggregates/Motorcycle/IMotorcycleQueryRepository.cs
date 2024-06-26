using BuildingBlocks.Domain.Repositories;

namespace Core.Domain.Aggregates.Motorcycle
{
    public interface IMotorcycleQueryRepository : IQueryRepository<Motorcycle>
    {
        Task<Motorcycle?> GetByPlateAsync(string plate);
        IQueryable<Motorcycle> SearchBy(string? plate);
    }
}
