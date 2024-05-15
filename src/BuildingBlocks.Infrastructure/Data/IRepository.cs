using BuildingBlocks.Domain;

namespace BuildingBlocks.Infrastructure.Data
{
    public interface IRepository<TEntity> where TEntity : IEntity
    {
    }
}
