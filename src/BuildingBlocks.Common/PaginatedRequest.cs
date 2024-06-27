namespace BuildingBlocks.API.Core.Data.Pagination
{
    public class PaginatedRequest
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 25;
    }
}
