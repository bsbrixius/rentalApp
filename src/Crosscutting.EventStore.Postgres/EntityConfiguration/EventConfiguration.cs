using Crosscutting.EventStore.Postgres.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crosscutting.EventStore.Postgres.EntityConfiguration
{
    public class EventConfiguration : IEntityTypeConfiguration<EventData>
    {
        public void Configure(EntityTypeBuilder<EventData> builder)
        {
            builder.ToTable("EventData", "event");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(e => e.Type)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(e => e.Data)
                .HasColumnType("jsonb")
                .IsRequired();

            builder.Property(e => e.Timestamp)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .IsRequired();

            builder.Property(e => e.Version)
                .IsRequired();

            builder.HasIndex(e => e.Timestamp);
        }
    }
}