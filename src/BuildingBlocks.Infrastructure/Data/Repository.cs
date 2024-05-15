using BuildingBlocks.Domain;

namespace BuildingBlocks.Infrastructure.Data
{
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class, IEntity
    {
    }
}
