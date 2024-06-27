using BuildingBlocks.Domain.Base;
using System.Linq.Expressions;

namespace BuildingBlocks.Domain.Repositories
{
    public interface IRepositoryBase<TEntity> where TEntity : IEntity
    {
        IQueryable<TEntity> QueryNoTrack { get; }
        Task<List<TEntity>> GetAllByAsync(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties);
        bool HasAny();
        bool HasAnyWith(Expression<Func<TEntity, bool>> property);
        Task<TEntity?> GetByIdAsync(Guid id);
    }
}
