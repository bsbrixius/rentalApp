using BuildingBlocks.API.Core.Data.Pagination;
using BuildingBlocks.Common.Enums;
using Core.Application.DTOs.Motorcycle;
using Core.Domain.Aggregates.Motorcycle;
using MediatR;
using static Core.Application.DTOs.Motorcycle.ListMotorcycleRequest;

namespace Core.Application.Query.Motorcycle
{
    public sealed class ListMotorcycleQuery : PaginatedRequest, IRequest<PaginatedResult<MotorcycleDTO>>
    {
        public OrderByType OrderBy { get; set; } = OrderByType.Year;
        public SortByType SortBy { get; set; } = SortByType.Descending;

        internal sealed class ListMotorcycleQueryHandler : IRequestHandler<ListMotorcycleQuery, PaginatedResult<MotorcycleDTO>>
        {
            private readonly IMotorcycleQueryRepository _motorcycleQueryRepository;

            public ListMotorcycleQueryHandler(IMotorcycleQueryRepository motorcycleQueryRepository)
            {
                _motorcycleQueryRepository = motorcycleQueryRepository;
            }
            public async Task<PaginatedResult<MotorcycleDTO>> Handle(ListMotorcycleQuery request, CancellationToken cancellationToken)
            {
                var query = _motorcycleQueryRepository.QueryNoTrack;

                query = query.Where(x => !x.IsDeleted);

                switch (request.OrderBy)
                {
                    case OrderByType.Year:
                        query = request.SortBy == SortByType.Ascending ? query.OrderBy(x => x.Year) : query.OrderByDescending(x => x.Year);
                        break;
                    case OrderByType.Model:
                        query = request.SortBy == SortByType.Ascending ? query.OrderBy(x => x.Model) : query.OrderByDescending(x => x.Model);
                        break;
                }
                return await query.PaginateAsync(request.Page, request.PageSize, MotorcycleDTO.FromNoPlate);
            }
        }
    }
}
