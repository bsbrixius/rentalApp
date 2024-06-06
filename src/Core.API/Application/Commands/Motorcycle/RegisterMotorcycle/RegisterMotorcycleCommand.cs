using MediatR;

namespace Core.API.Application.Commands.Motorcycle.RegisterMotorcycle
{
    public class RegisterMotorcycleCommand : IRequest<Guid>
    {
        public required uint Year { get; set; }
        public required string Model { get; set; }
        public required string Plate { get; set; }
    }
}
