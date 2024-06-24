namespace BuildingBlocks.API.Core.Data.Pagination
{
    public interface IPaginatedResult<T>
    {
        IEnumerable<T> Items { get; set; }
        int PageSize { get; set; }
        int Page { get; set; }
        int? TotalItems { get; set; }
        public int TotalPages { get; set; }
    }
    public class PaginatedResult<T> : IPaginatedResult<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }
        public int? TotalItems { get; set; }
        public int TotalPages { get; set; }
    }
}
