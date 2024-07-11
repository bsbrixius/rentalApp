using BuildingBlocks.Domain.Repositories;
using Core.Domain.Aggregates.Rent;

namespace Core.Data.Repositories
{
    public class RentQueryRepository : QueryRepository<Rent, CoreContext>, IRentQueryRepository
    {
        public RentQueryRepository(CoreContext context) : base(context)
        {
        }
    }
}
