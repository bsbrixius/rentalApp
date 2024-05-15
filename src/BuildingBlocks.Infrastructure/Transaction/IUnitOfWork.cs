namespace BuildingBlocks.Infrastructure.Data
{
    public interface IUnitOfWork
    {
        void Commit();
        Task CommitAsync();
    }
}
