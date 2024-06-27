using BuildingBlocks.Eventbus;

namespace Core.Application.IntegrationEvents.Events.Motorcycle
{
    public record MotorcycleRegisteredIntegrationEvent : IntegrationEvent
    {
        public Guid Id { get; set; }
        public uint Year { get; set; }
        public string Model { get; set; }
        public string Plate { get; set; }
    }
}
