namespace BuildingBlocks.API.Core.Data.Pagination
{
    public class PaginatedRequest
    {
        public int Page { get; set; } = 0;
        public int PageSize { get; set; } = 25;
    }
}
