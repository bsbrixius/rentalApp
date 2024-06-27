using Ardalis.GuardClauses;
namespace BuildingBlocks.API.Core.Data.Pagination
{
    public static class Extensions
    {
        public async static Task<PaginatedResult<T>> PaginateAsync<T, TEntity>(this IQueryable<TEntity> queriable, int pageNumber, int pageSize, Func<TEntity, T> fromConverter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null)
            where T : class
            where TEntity : class
        {
            Guard.Against.Null(queriable, nameof(queriable));
            Guard.Against.NegativeOrZero(pageNumber, nameof(pageNumber));
            Guard.Against.NegativeOrZero(pageSize, nameof(pageSize));

            var totalItems = queriable.Count();
            if (totalItems > 0)
            {
                if (orderBy != null)
                    queriable = orderBy(queriable);

                var items = queriable.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                return new PaginatedResult<T>
                {
                    Items = items.Select(x => fromConverter(x)).ToList(),
                    PageSize = pageSize,
                    Page = pageNumber,
                    TotalItems = totalItems
                };
            }
            return new PaginatedResult<T>
            {
                Items = new List<T>(),
                PageSize = pageSize,
                Page = pageNumber,
                TotalItems = 0
            };
        }

        public async static Task<PaginatedResult<T>> PaginateAsync<T>(this IQueryable<T> queriable, int pageNumber, int pageSize, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null)
            where T : class
        {
            Guard.Against.Null(queriable, nameof(queriable));
            Guard.Against.NegativeOrZero(pageNumber, nameof(pageNumber));
            Guard.Against.NegativeOrZero(pageSize, nameof(pageSize));

            var totalItems = queriable.Count();
            if (totalItems > 0)
            {
                if (orderBy != null)
                    queriable = orderBy(queriable);

                var items = queriable.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                return new PaginatedResult<T>
                {
                    Items = items,
                    PageSize = pageSize,
                    Page = pageNumber,
                    TotalItems = totalItems
                };
            }
            return new PaginatedResult<T>
            {
                Items = new List<T>(),
                PageSize = pageSize,
                Page = pageNumber,
                TotalItems = 0
            };
        }
    }
}
