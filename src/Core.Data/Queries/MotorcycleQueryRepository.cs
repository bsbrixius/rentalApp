﻿using BuildingBlocks.Domain.Repositories;
using Core.Domain.Aggregates.Motorcycle;
using Microsoft.EntityFrameworkCore;

namespace Core.Data.Queries
{
    public class MotorcycleQueryRepository : QueryRepository<Motorcycle, CoreContext>, IMotorcycleQueryRepository
    {
        public MotorcycleQueryRepository(CoreContext context) : base(context)
        {
        }

        public async Task<Motorcycle?> GetAvailableAtAsync(DateOnly startAt)
        {
            return await QueryNoTrack.FirstOrDefaultAsync(x => !x.IsDeleted && !x.Rents.Any(r => r.EndAt > startAt));
        }

        public async Task<Motorcycle?> GetByPlateAsync(string plate)
        {
            return await QueryNoTrack.FirstOrDefaultAsync(x => x.Plate == plate);
        }

        public IQueryable<Motorcycle> SearchByPlate(string? plate)
        {
            var sql = string.Format(
                "SELECT * FROM \"Motorcycles\" WHERE \"Plate\" LIKE '%{0}%'",
                plate);
            var sqlRaw = _dbSet.FromSqlRaw(sql);
            return sqlRaw;
        }
    }
}
