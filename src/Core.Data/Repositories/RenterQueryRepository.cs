using BuildingBlocks.Domain.Repositories;
using Core.Domain.Aggregates.Renter;

namespace Core.Data.Repositories
{
    public class RenterQueryRepository : QueryRepository<Renter, CoreContext>, IRenterQueryRepository
    {
        public RenterQueryRepository(CoreContext context) : base(context)
        {
        }
    }
}
