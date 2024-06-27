using System.Text.Json.Serialization;

namespace BuildingBlocks.Eventbus
{
    public record IntegrationEvent
    {
        public IntegrationEvent()
        {
            EventId = Guid.NewGuid();
            CreationDate = DateTime.UtcNow;
        }

        [JsonConstructor]
        public IntegrationEvent(Guid id, DateTime createDate, Guid correlationId)
        {
            EventId = id;
            CreationDate = createDate;
            CorrelationId = correlationId;
        }

        [JsonInclude]
        public Guid EventId { get; private init; }
        [JsonInclude]
        public Guid CorrelationId { get; init; }

        [JsonInclude]
        public DateTime CreationDate { get; private init; }
    }
}
