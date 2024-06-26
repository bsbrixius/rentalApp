using BuildingBlocks.Domain.Base;
using System.Linq.Expressions;

namespace BuildingBlocks.Domain.Repositories
{
    public interface IRepository<TEntity> : IRepositoryBase<TEntity> where TEntity : IEntity
    {
        Task<bool> CommitAsync();
        void Update(TEntity entity);
        void UpdateRange(List<TEntity> entities);
        void DeleteBy(Expression<Func<TEntity, bool>> query);
        void DeleteById(Guid id);
        Task<bool> SaveChangesAsync();
        Task<Guid> AddAsync(TEntity entity);
        Task AddRangeAsync(List<TEntity> entities);
    }
}
