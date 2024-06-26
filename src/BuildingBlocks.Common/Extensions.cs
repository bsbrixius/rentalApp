using Ardalis.GuardClauses;
namespace BuildingBlocks.API.Core.Data.Pagination
{
    public static class Extensions
    {
        public async static Task<PaginatedResult<T>> PaginateAsync<T, TEntity>(this IQueryable<TEntity> query, int pageNumber, int pageSize, Func<TEntity, T> fromConverter)
        {
            Guard.Against.Null(query, nameof(query));
            Guard.Against.NegativeOrZero(pageNumber, nameof(pageNumber));
            Guard.Against.NegativeOrZero(pageSize, nameof(pageSize));

            var itemsQuery = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            return new PaginatedResult<T>
            {
                Items = itemsQuery.Select(x => fromConverter(x)).ToList(),
                PageSize = pageSize,
                Page = pageNumber,
                TotalItems = null
            };
        }

        public async static Task<PaginatedResult<T>> PaginateAsync<T>(this IQueryable<T> query, int pageNumber, int pageSize)
        {
            Guard.Against.Null(query, nameof(query));
            Guard.Against.NegativeOrZero(pageNumber, nameof(pageNumber));
            Guard.Against.NegativeOrZero(pageSize, nameof(pageSize));

            var items = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return new PaginatedResult<T>
            {
                Items = items,
                PageSize = pageSize,
                Page = pageNumber,
                TotalItems = null
            };
        }
    }
}
