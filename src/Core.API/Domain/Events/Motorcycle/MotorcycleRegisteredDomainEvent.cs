
using BuildingBlocks.EventSourcing;
using Core.API.Application.Data.DTOs.Motorcycle;

namespace Core.API.Domain.Events.Motorcycle
{
    public class MotorcycleRegisteredDomainEvent : DomainEvent
    {
        public MotorcycleRegisteredDomainEvent(Domain.Motorcycle motorcycle)
        {
            Motorcycle = new MotorcycleDTO
            {
                Id = motorcycle.Id,
                Year = motorcycle.Year,
                Model = motorcycle.Model,
                Plate = motorcycle.Plate
            };
        }

        public MotorcycleDTO Motorcycle { get; private set; }
    }
}
