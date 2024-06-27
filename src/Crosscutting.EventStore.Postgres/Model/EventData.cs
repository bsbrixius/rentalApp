namespace Crosscutting.EventStore.Postgres.Model
{
    public class EventData
    {
        //public EventData(Guid eventId, Guid aggregateId, string type, string data, DateTimeOffset? timestamp = null)
        //{
        //    EventId = eventId;
        //    AggregateId = aggregateId;
        //    Type = type;
        //    Data = data;
        //    Timestamp = timestamp ?? DateTimeOffset.UtcNow;
        //}

        public int Id { get; set; }
        public Guid EventId { get; set; }
        public Guid AggregateId { get; set; }
        public string Type { get; set; }
        public string Data { get; set; }
        public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.UtcNow;
        public int Version { get; set; } = 1;
    }
}
