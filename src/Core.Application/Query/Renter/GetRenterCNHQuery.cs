using Core.Domain.Aggregates.Renter;
using Crosscutting.StorageService.Base;
using MediatR;

namespace Core.Application.Query.Renter
{
    public sealed class GetRenterCNHQuery : IRequest<string>
    {
        public Guid UserId { get; set; }
        internal sealed class GetRenterCNHQueryHandler : IRequestHandler<GetRenterCNHQuery, string>
        {
            private readonly IRenterQueryRepository _renterQueryRepository;
            private readonly IStorageService _storageService;

            public GetRenterCNHQueryHandler(
                IRenterQueryRepository renterQueryRepository,
                IStorageService storageService)
            {
                _renterQueryRepository = renterQueryRepository;
                _storageService = storageService;
            }

            public async Task<string> Handle(GetRenterCNHQuery request, CancellationToken cancellationToken)
            {
                var renter = await _renterQueryRepository.GetByUserIdAsync(request.UserId);
                var cnhUrl = await _storageService.GetAsync("renter", renter.CNH.Url);
                return cnhUrl;
            }
        }
    }
}
