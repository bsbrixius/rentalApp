using BuildingBlocks.Domain.Base;

namespace BuildingBlocks.Domain.Repositories
{
    public interface IQueryRepository<TEntity> : IRepositoryBase<TEntity> where TEntity : IEntity
    {

    }
}
