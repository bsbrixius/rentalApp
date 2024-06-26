using BuildingBlocks.Domain.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BuildingBlocks.Domain.Repositories
{
    public class Repository<TEntity, TDbContext> : RepositoryBase<TEntity, TDbContext>, IRepository<TEntity>
        where TEntity : Entity
        where TDbContext : DbContext
    {
        public Repository(TDbContext context) : base(context)
        {
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
    }
}
