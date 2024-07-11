using BuildingBlocks.Domain.Repositories;
using Core.Domain.Aggregates.Renter;
using Microsoft.EntityFrameworkCore;

namespace Core.Data.Repositories
{
    public class RenterQueryRepository : QueryRepository<Renter, CoreContext>, IRenterQueryRepository
    {
        public RenterQueryRepository(CoreContext context) : base(context)
        {
        }

        public async Task<Renter?> GetByUserIdAsync(Guid userId)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.UserId == userId);
        }
    }
}
