using BuildingBlocks.Domain.Repositories;
using Core.Domain.Aggregates.Renter;

namespace Core.Data.Repositories
{
    public class RenterRepository : Repository<Renter, CoreContext>, IRenterRepository
    {
        public RenterRepository(CoreContext context) : base(context)
        {
        }
    }
}