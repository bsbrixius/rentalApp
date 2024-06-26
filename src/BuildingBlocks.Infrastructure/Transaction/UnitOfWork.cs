using BuildingBlocks.Domain.Base;
using BuildingBlocks.Infrastructure.Data;

namespace BuildingBlocks.Infrastructure.Transaction
{
    public class UnitOfWork<TDbContext> : IUnitOfWork
        where TDbContext : BaseDb
    {
        private readonly TDbContext _context;

        public UnitOfWork(TDbContext context)
        {
            _context = context;
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
