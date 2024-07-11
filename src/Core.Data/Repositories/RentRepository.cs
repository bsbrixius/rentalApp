using BuildingBlocks.Domain.Repositories;
using Core.Domain.Aggregates.Rent;

namespace Core.Data.Repositories
{
    public class RentRepository : Repository<Rent, CoreContext>, IRentRepository
    {
        public RentRepository(CoreContext context) : base(context)
        {
        }
    }
}
