using BuildingBlocks.API.Core.Data.Pagination;
using Core.Application.Data.DTOs.Motorcycle;
using Core.Domain.Aggregates.Motorcycle;
using MediatR;

namespace Core.Application.Query.Motorcycle
{
    public sealed class SearchMotorcycleQuery : PaginatedRequest, IRequest<PaginatedResult<MotorcycleDTO>>
    {
        public string? Plate { get; set; }

        internal sealed class SearchMotorcycleQueryHandler : IRequestHandler<SearchMotorcycleQuery, PaginatedResult<MotorcycleDTO>>
        {
            private readonly IMotorcycleQueryRepository _motorcycleQueryRepository;

            public SearchMotorcycleQueryHandler(IMotorcycleQueryRepository motorcycleQueryRepository)
            {
                _motorcycleQueryRepository = motorcycleQueryRepository;
            }
            public async Task<PaginatedResult<MotorcycleDTO>> Handle(SearchMotorcycleQuery request, CancellationToken cancellationToken)
            {
                return await _motorcycleQueryRepository.SearchBy(request.Plate).PaginateAsync(request.Page, request.PageSize, MotorcycleDTO.From);
            }
        }
    }
}
