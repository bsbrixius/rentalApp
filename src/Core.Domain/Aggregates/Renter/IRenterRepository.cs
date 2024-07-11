using BuildingBlocks.Domain.Repositories;

namespace Core.Domain.Aggregates.Renter
{
    public interface IRenterRepository : IRepository<Renter>
    {
        Task<Renter?> GetByUserIdAsync(Guid userId);
    }
}
