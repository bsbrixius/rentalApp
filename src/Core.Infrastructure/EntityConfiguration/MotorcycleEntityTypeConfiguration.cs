using BuildingBlocks.Infrastructure.Configuration;
using Core.Domain.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Infrastructure.EntityConfiguration
{
    public class MotorcycleEntityTypeConfiguration : BaseEntityTypeConfiguration<Motorcycle>
    {
        public override void Configure(EntityTypeBuilder<Motorcycle> builder)
        {
            builder.Property(m => m.Year).IsRequired();
            builder.Property(m => m.Model).IsRequired();
            builder.Property(m => m.Plate).IsRequired();
            base.Configure(builder);
        }
    }
}
