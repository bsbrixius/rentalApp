using BuildingBlocks.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BuildingBlocks.Infrastructure.Data
{
    public class Repository<TEntity, TDbContext> : IRepository<TEntity>
        where TEntity : Entity
        where TDbContext : DbContext

    {
        private readonly TDbContext _context;
        protected DbSet<TEntity> _dbSet { get; }
        public IQueryable<TEntity> QueryNoTrack => _dbSet.AsNoTracking();

        public Repository(TDbContext context)
        {
            _dbSet = context.Set<TEntity>();
        }

        public virtual async Task<Guid> AddAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            await _dbSet.AddAsync(entity);
            return entity.Id;
        }

        public virtual async Task AddRangeAsync(List<TEntity> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            await _dbSet.AddRangeAsync(entities);
        }


        public async Task<bool> CommitAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void DeleteBy(Expression<Func<TEntity, bool>> query)
        {
            _dbSet.Where(query).DeleteFromQuery();
        }

        public void DeleteById(Guid id)
        {
            _dbSet.Where(x => x.Id == id).DeleteFromQuery();
        }

        public async Task<List<TEntity>> GetAllByAsync(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties)
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

        public async Task<TEntity> GetByIdAsync(Guid id)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.Id == id);
        }

        public bool HasAny()
        {
            return QueryNoTrack.Any();
        }

        public bool HasAnyWith(Expression<Func<TEntity, bool>> property)
        {
            return _dbSet.Any(property);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(TEntity entity)
        {
            _dbSet.Update(entity);
        }

        public void UpdateRange(List<TEntity> entities)
        {
            _dbSet.UpdateRange(entities);
        }

        #region Private
        private IQueryable<TEntity> Include(IQueryable<TEntity> query, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            foreach (Expression<Func<TEntity, object>> property in includeProperties)
            {
                query = query.Include(property);
            }

            return query;
        }
        #endregion
    }
}
