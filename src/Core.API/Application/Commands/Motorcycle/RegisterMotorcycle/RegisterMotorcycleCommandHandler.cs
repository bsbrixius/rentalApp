using Core.API.Application.Data.Repositories;
using MediatR;

namespace Core.API.Application.Commands.Motorcycle.RegisterMotorcycle
{
    public class RegisterMotorcycleCommandHandler : IRequestHandler<RegisterMotorcycleCommand, Guid>
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
