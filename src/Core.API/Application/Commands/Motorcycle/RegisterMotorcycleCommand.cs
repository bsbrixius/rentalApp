using Core.API.Domain.Repositories;
using MediatR;

namespace Core.API.Application.Commands.Motorcycle
{
    public sealed class RegisterMotorcycleCommand : IRequest<Guid>
    {
        public required uint Year { get; set; }
        public required string Model { get; set; }
        public required string Plate { get; set; }

        internal sealed class RegisterMotorcycleCommandHandler : IRequestHandler<RegisterMotorcycleCommand, Guid>
        {
            private readonly IMotorcycleRepository _motorcycleRepository;

            public RegisterMotorcycleCommandHandler(IMotorcycleRepository motorcycleRepository)
            {
                _motorcycleRepository = motorcycleRepository;
            }

            public async Task<Guid> Handle(RegisterMotorcycleCommand request, CancellationToken cancellationToken)
            {
                var newMotorcycle = new Domain.Motorcycle(request.Year, request.Model, request.Plate);

                return await _motorcycleRepository.AddAsync(newMotorcycle);
            }
        }
    }
}
