using Core.Domain.Aggregates.Motorcycle;
using FluentValidation;
using MediatR;

namespace Core.Application.Commands.Motorcycle
{
    public sealed class RegisterMotorcycleCommand : IRequest
    {
        public required uint Year { get; set; }
        public required string Model { get; set; }
        public required string Plate { get; set; }

        internal sealed class RegisterMotorcycleCommandHandler : IRequestHandler<RegisterMotorcycleCommand>
        {
            private readonly IMotorcycleRepository _motorcycleRepository;

            public RegisterMotorcycleCommandHandler(IMotorcycleRepository motorcycleRepository)
            {
                _motorcycleRepository = motorcycleRepository;
            }

            public async Task Handle(RegisterMotorcycleCommand request, CancellationToken cancellationToken)
            {
                var newMotorcycle = new Domain.Aggregates.Motorcycle.Motorcycle(request.Year, request.Model, request.Plate);

                await _motorcycleRepository.AddAsync(newMotorcycle);
                await _motorcycleRepository.SaveChangesAsync();
            }
        }
    }

    public class RegisterMotorcycleCommandValidator : AbstractValidator<RegisterMotorcycleCommand>
    {
        public RegisterMotorcycleCommandValidator()
        {
            RuleFor(x => x.Year).GreaterThan((uint)1999);
            RuleFor(x => x.Model).NotEmpty().NotNull();
            RuleFor(x => x.Plate).NotEmpty().NotNull();
        }
    }
}
