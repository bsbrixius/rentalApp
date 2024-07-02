using BuildingBlocks.API.Core.Data.Pagination;
using BuildingBlocks.Common.Enums;

namespace Core.Application.DTOs.Motorcycle
{
    public class SearchMotorcycleRequest : PaginatedRequest
    {
        public enum OrderByType
        {
            Year = 0,
            Model = 1,
            Plate = 2
        }
        public string? Plate { get; set; }
        public OrderByType OrderBy { get; set; } = OrderByType.Year;
        public SortByType SortBy { get; set; } = SortByType.Descending;
    }
}
