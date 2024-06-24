using BuildingBlocks.Domain;
using System.Linq.Expressions;

namespace BuildingBlocks.Infrastructure.Data
{
    public interface IRepository<TEntity> where TEntity : IEntity
    {
        IQueryable<TEntity> QueryNoTrack { get; }
        Task<List<TEntity>> GetAllByAsync(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties);
        Task<bool> CommitAsync();
        Task<TEntity> GetByIdAsync(Guid id);
        bool HasAny();
        void Update(TEntity entity);
        void UpdateRange(List<TEntity> entities);
        void DeleteBy(Expression<Func<TEntity, bool>> query);
        void DeleteById(Guid id);
        Task<bool> SaveChangesAsync();
        bool HasAnyWith(Expression<Func<TEntity, bool>> property);
        Task<Guid> AddAsync(TEntity entity);
        Task AddRangeAsync(List<TEntity> entities);
    }
}
