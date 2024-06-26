using BuildingBlocks.Infrastructure.Configuration;
using Core.Domain.Aggregates.Motorcycle;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Infraestructure.EntityConfiguration
{
    public class MotorcycleConfiguration : BaseEntityTypeConfiguration<Motorcycle>
    {
        public override void Configure(EntityTypeBuilder<Motorcycle> builder)
        {
            builder.Property(m => m.Year).IsRequired();
            builder.Property(m => m.Model).IsRequired();
            builder.Property(m => m.Plate).IsRequired();
            builder.HasIndex(m => m.Plate).IsUnique();

            base.Configure(builder);
        }
    }
}
