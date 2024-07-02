using BuildingBlocks.Infrastructure.Exceptions;
using Core.Domain.Aggregates.Motorcycle;
using MediatR;

namespace Core.Application.Commands.Motorcycle
{
    public sealed class UpdateMotorcycleCommand : IRequest
    {
        public Guid Id { get; set; }
        public string Plate { get; set; }

        internal sealed class UpdateMotorcycleCommandHandler : IRequestHandler<UpdateMotorcycleCommand>
        {
            private readonly IMotorcycleRepository _motorcycleRepository;

            public UpdateMotorcycleCommandHandler(IMotorcycleRepository motorcycleRepository)
            {
                _motorcycleRepository = motorcycleRepository;
            }

            public async Task Handle(UpdateMotorcycleCommand request, CancellationToken cancellationToken)
            {
                var motorcycle = await _motorcycleRepository.GetByIdAsync(request.Id);

                if (motorcycle == null)
                {
                    throw new NotFoundException($"The motorcycle with id {request.Id} was not found.");
                }

                motorcycle.SetPlate(request.Plate);

                await _motorcycleRepository.SaveChangesAsync();
            }
        }

    }
}
