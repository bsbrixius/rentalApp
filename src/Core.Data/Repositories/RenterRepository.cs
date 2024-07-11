using BuildingBlocks.Domain.Repositories;
using Core.Domain.Aggregates.Renter;
using Microsoft.EntityFrameworkCore;

namespace Core.Data.Repositories
{
    public class RenterRepository : Repository<Renter, CoreContext>, IRenterRepository
    {
        public RenterRepository(CoreContext context) : base(context)
        {
        }
        public async Task<Renter?> GetByUserIdAsync(Guid userId)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.UserId == userId);
        }
    }
}