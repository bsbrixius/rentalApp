using BuildingBlocks.Infrastructure.Exceptions;
using Core.Domain.Aggregates.Renter;
using MediatR;

namespace Core.Application.Commands.Renter
{
    public class RegisterRenterCommand : IRequest
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string CNPJ { get; set; }
        public DateOnly BirthDay { get; set; }
        internal sealed class RegisterRenterCommandHandler : IRequestHandler<RegisterRenterCommand>
        {
            private readonly IRenterRepository _renterRepository;

            public RegisterRenterCommandHandler(IRenterRepository renterRepository)
            {
                _renterRepository = renterRepository;
            }

            public async Task Handle(RegisterRenterCommand request, CancellationToken cancellationToken)
            {
                if (_renterRepository.HasAnyWith(x => x.CNPJ == request.CNPJ))
                {
                    throw new DomainException($"Renter already exists with CNPJ: {request.CNPJ}");
                }

                var renter = new Domain.Aggregates.Renter.Renter(request.UserId, request.Name, request.CNPJ, request.BirthDay);
                await _renterRepository.AddAsync(renter);
                await _renterRepository.SaveChangesAsync();
            }
        }
    }
}
