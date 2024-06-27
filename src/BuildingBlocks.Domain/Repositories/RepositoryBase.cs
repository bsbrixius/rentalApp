using BuildingBlocks.Domain.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BuildingBlocks.Domain.Repositories
{
    public class RepositoryBase<TEntity, TDbContext> : IRepositoryBase<TEntity>
        where TEntity : Entity
        where TDbContext : DbContext
    {
        protected readonly TDbContext _context;
        protected DbSet<TEntity> _dbSet { get; }
        public IQueryable<TEntity> QueryNoTrack => _dbSet.AsNoTracking();

        public RepositoryBase(TDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }
        public virtual async Task<List<TEntity>> GetAllByAsync(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

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

        public virtual async Task<TEntity?> GetByIdAsync(Guid id)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.Id == id);
        }

        public bool HasAny()
        {
            return QueryNoTrack.Any();
        }

        public bool HasAnyWith(Expression<Func<TEntity, bool>> property)
        {
            return QueryNoTrack.Any(property);
        }

        private protected IQueryable<TEntity> Include(IQueryable<TEntity> query, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            foreach (Expression<Func<TEntity, object>> property in includeProperties)
            {
                query = query.Include(property);
            }

            return query;
        }
    }
}
