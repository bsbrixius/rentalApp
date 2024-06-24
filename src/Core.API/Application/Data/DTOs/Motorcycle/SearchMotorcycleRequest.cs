using BuildingBlocks.API.Core.Data.Pagination;

namespace Core.API.Application.Data.DTOs.Motorcycle
{
    public class SearchMotorcycleRequest : PaginatedRequest
    {
        public string? Plate { get; set; }
    }
}
