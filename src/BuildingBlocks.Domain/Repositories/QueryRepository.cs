using BuildingBlocks.Domain.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BuildingBlocks.Domain.Repositories
{
    public class QueryRepository<TEntity, TDbContext> : RepositoryBase<TEntity, TDbContext>, IQueryRepository<TEntity>
        where TEntity : Entity
        where TDbContext : DbContext
    {
        public QueryRepository(TDbContext context) : base(context)
        {
        }

        public override async Task<List<TEntity>> GetAllByAsync(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = QueryNoTrack;

            if (includeProperties.Any())
            {
                query = Include(query, includeProperties);
            }

            if (where != null)
            {
                query = query.Where(where);
            }

            return await query.ToListAsync();
        }

        public override async Task<TEntity?> GetByIdAsync(Guid id)
        {
            return await QueryNoTrack.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
