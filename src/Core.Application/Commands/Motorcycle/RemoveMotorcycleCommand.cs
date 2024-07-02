using BuildingBlocks.Infrastructure.Exceptions;
using Core.Domain.Aggregates.Motorcycle;
using MediatR;

namespace Core.Application.Commands.Motorcycle
{
    public sealed class RemoveMotorcycleCommand : IRequest
    {
        public Guid Id { get; set; }
        internal sealed class RemoveMotorcycleCommandHandler : IRequestHandler<RemoveMotorcycleCommand>
        {
            private readonly IMotorcycleRepository _motorcycleRepository;

            public RemoveMotorcycleCommandHandler(IMotorcycleRepository motorcycleRepository)
            {
                _motorcycleRepository = motorcycleRepository;
            }

            public async Task Handle(RemoveMotorcycleCommand request, CancellationToken cancellationToken)
            {
                var motorcycle = await _motorcycleRepository.GetByIdAsync(request.Id);
                if (motorcycle == null)
                    throw new NotFoundException($"The motorcycle with id {request.Id} was not found.");

                if (_motorcycleRepository.HasAnyRent(request.Id))
                    throw new ConflictException($"Can not delete the motorcycle with id {request.Id}. Because it has Rents related");

                motorcycle.SoftDelete();
                _motorcycleRepository.Update(motorcycle);

                await _motorcycleRepository.SaveChangesAsync();
            }
        }
    }
}
