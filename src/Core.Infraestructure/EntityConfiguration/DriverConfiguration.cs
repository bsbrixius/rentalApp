using BuildingBlocks.Infrastructure.Configuration;
using Core.Domain.Aggregates.Driver;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Infraestructure.EntityConfiguration
{
    public class DriverConfiguration : BaseEntityTypeConfiguration<Driver>
    {
        public override void Configure(EntityTypeBuilder<Driver> builder)
        {
            builder.Property(d => d.Name).IsRequired();
            builder.Property(d => d.CNPJ).IsRequired();
            builder.Property(d => d.Birthdate).IsRequired();
            builder.Property(d => d.CNH).IsRequired();
            builder.Property(d => d.CNHCategory).IsRequired();
            builder.Property(d => d.CNHUrl).IsRequired();
            base.Configure(builder);
        }
    }
}
