using BuildingBlocks.Domain.Repositories;

namespace Core.Domain.Aggregates.Motorcycle
{
    public interface IMotorcycleQueryRepository : IQueryRepository<Motorcycle>
    {
        Task<Motorcycle?> GetByPlateAsync(string plate);
        IQueryable<Motorcycle> SearchByPlate(string? plate);
        Task<Motorcycle?> GetAvailableAtAsync(DateOnly startAt);
    }
}
