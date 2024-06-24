using BuildingBlocks.API.Core.Data.Pagination;
using Core.API.Application.Data.DTOs.Motorcycle;
using MediatR;

namespace Core.API.Application.Query.Motorcycle
{
    public sealed class SearchMotorcycleQuery : PaginatedRequest, IRequest<PaginatedResult<MotorcycleDTO>>
    {
        public string? Plate { get; set; }

        internal sealed class SearchMotorcycleQueryHandler : IRequestHandler<SearchMotorcycleQuery, PaginatedResult<MotorcycleDTO>>
        {
            private readonly IMotorcycleQueries _motorcycleQueries;

            public SearchMotorcycleQueryHandler(IMotorcycleQueries motorcycleQueries)
            {
                _motorcycleQueries = motorcycleQueries;
            }
            public async Task<PaginatedResult<MotorcycleDTO>> Handle(SearchMotorcycleQuery request, CancellationToken cancellationToken)
            {
                return await _motorcycleQueries.SearchMotorcycleAsync(request);
            }
        }
    }
}
