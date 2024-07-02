using Core.Application.DTOs.Renter;
using Core.Domain.Aggregates.Renter;
using MediatR;

namespace Core.Application.Query.Renter
{
    public sealed class GetRenterQuery : IRequest<RenterDTO?>
    {
        public Guid Id { get; set; }
        internal sealed class GetRenterQueryHandler : IRequestHandler<GetRenterQuery, RenterDTO?>
        {
            private readonly IRenterQueryRepository _renterQueryRepository;

            public GetRenterQueryHandler(IRenterQueryRepository renterQueryRepository)
            {
                _renterQueryRepository = renterQueryRepository;
            }

            public async Task<RenterDTO?> Handle(GetRenterQuery request, CancellationToken cancellationToken)
            {
                return RenterDTO.From(await _renterQueryRepository.GetByIdAsync(request.Id));
            }
        }
    }
}
