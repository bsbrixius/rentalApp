using BuildingBlocks.Domain.Repositories;
using Core.Domain.Aggregates.Motorcycle;
using Microsoft.EntityFrameworkCore;

namespace Core.Data.Repositories
{
    public class MotorcycleRepository : Repository<Motorcycle, CoreContext>, IMotorcycleRepository
    {
        public MotorcycleRepository(CoreContext context) : base(context)
        {
        }

        public async Task<Motorcycle?> GetAvailableAtAsync(DateOnly startAt)
        {
            return await QueryNoTrack.FirstOrDefaultAsync(x => !x.IsDeleted && !x.Rents.Any(r => r.EndAt > startAt));
        }

        public bool HasAnyRent(Guid id)
        {
            return _dbSet.Any(x => x.Rents.Any());
        }
    }
}
