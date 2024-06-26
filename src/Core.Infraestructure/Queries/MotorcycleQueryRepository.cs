using BuildingBlocks.Domain.Repositories;
using Core.Domain.Aggregates.Motorcycle;
using Microsoft.EntityFrameworkCore;

namespace Core.Infraestructure.Queries
{
    public class MotorcycleQueryRepository : QueryRepository<Motorcycle, CoreContext>, IMotorcycleQueryRepository
    {
        public MotorcycleQueryRepository(CoreContext context) : base(context)
        {
        }

        public async Task<Motorcycle?> GetByPlateAsync(string plate)
        {
            return await QueryNoTrack.FirstOrDefaultAsync(x => x.Plate == plate);
        }

        public IQueryable<Motorcycle> SearchBy(string? plate)
        {
            return QueryNoTrack.Where(x => EF.Functions.Like(x.Plate, $"{plate}"));
        }
    }
}
