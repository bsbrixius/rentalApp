using BuildingBlocks.API.Core.Data.Pagination;

namespace Core.Application.Data.DTOs.Motorcycle
{
    public class SearchMotorcycleRequest : PaginatedRequest
    {
        public string? Plate { get; set; }
    }
}
