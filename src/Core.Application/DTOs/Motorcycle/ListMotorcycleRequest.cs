using BuildingBlocks.API.Core.Data.Pagination;
using BuildingBlocks.Common.Enums;

namespace Core.Application.DTOs.Motorcycle
{
    public class ListMotorcycleRequest : PaginatedRequest
    {
        public enum OrderByType
        {
            Year = 0,
            Model = 1
        }
        public OrderByType OrderBy { get; set; } = OrderByType.Year;
        public SortByType SortBy { get; set; } = SortByType.Descending;
    }
}
