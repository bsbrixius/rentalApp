using BuildingBlocks.Domain.Repositories;

namespace Core.Domain.Aggregates.Renter
{
    public interface IRenterQueryRepository : IQueryRepository<Renter>
    {
        Task<Renter?> GetByUserIdAsync(Guid userId);
    }
}
