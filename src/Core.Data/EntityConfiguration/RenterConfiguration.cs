using BuildingBlocks.Infrastructure.Configuration;
using Core.Domain.Aggregates.Renter;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Data.EntityConfiguration
{
    public class RenterConfiguration : AuditableEntityTypeConfiguration<Renter>
    {
        public override void Configure(EntityTypeBuilder<Renter> builder)
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
