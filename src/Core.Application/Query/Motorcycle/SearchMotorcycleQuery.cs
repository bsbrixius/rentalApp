using BuildingBlocks.API.Core.Data.Pagination;
using BuildingBlocks.Common.Enums;
using Core.Application.DTOs.Motorcycle;
using Core.Domain.Aggregates.Motorcycle;
using MediatR;
using static Core.Application.DTOs.Motorcycle.SearchMotorcycleRequest;

namespace Core.Application.Query.Motorcycle
{
    public sealed class SearchMotorcycleQuery : PaginatedRequest, IRequest<PaginatedResult<MotorcycleDTO>>
    {
        public string? Plate { get; set; }
        public OrderByType OrderBy { get; set; } = OrderByType.Year;
        public SortByType SortBy { get; set; } = SortByType.Descending;

        internal sealed class SearchMotorcycleQueryHandler : IRequestHandler<SearchMotorcycleQuery, PaginatedResult<MotorcycleDTO>>
        {
            private readonly IMotorcycleQueryRepository _motorcycleQueryRepository;

            public SearchMotorcycleQueryHandler(IMotorcycleQueryRepository motorcycleQueryRepository)
            {
                _motorcycleQueryRepository = motorcycleQueryRepository;
            }
            public async Task<PaginatedResult<MotorcycleDTO>> Handle(SearchMotorcycleQuery request, CancellationToken cancellationToken)
            {
                var query = _motorcycleQueryRepository.SearchBy(request.Plate);
                switch (request.OrderBy)
                {
                    case OrderByType.Year:
                        query = request.SortBy == SortByType.Ascending ? query.OrderBy(x => x.Year) : query.OrderByDescending(x => x.Year);
                        break;
                    case OrderByType.Model:
                        query = request.SortBy == SortByType.Ascending ? query.OrderBy(x => x.Model) : query.OrderByDescending(x => x.Model);
                        break;
                    case OrderByType.Plate:
                        query = request.SortBy == SortByType.Ascending ? query.OrderBy(x => x.Plate) : query.OrderByDescending(x => x.Plate);
                        break;
                }
                return await query.PaginateAsync(request.Page, request.PageSize, MotorcycleDTO.From);
            }
        }
    }
}
