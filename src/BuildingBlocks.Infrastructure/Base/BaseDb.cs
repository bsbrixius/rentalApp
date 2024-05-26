using Microsoft.EntityFrameworkCore;

namespace BuildingBlocks.Infrastructure.Base
{
    public class BaseDb : DbContext
    {
        public BaseDb(DbContextOptions options) : base(options)
        {

        }
    }
}
