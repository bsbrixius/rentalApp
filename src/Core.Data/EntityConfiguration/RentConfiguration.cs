using BuildingBlocks.Infrastructure.Configuration;
using Core.Domain.Aggregates.Rent;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Data.EntityConfiguration
{
    public class RentConfiguration : AuditableEntityTypeConfiguration<Rent>
    {
        public override void Configure(EntityTypeBuilder<Rent> builder)
        {

            builder.HasIndex(x => x.MotorcycleId);
            builder.Property(x => x.MotorcycleId).IsRequired();

            builder.HasIndex(x => x.RenterId);
            builder.Property(x => x.RenterId).IsRequired();

            builder.HasIndex(x => new { x.StartAt, x.EndAt });

            builder.Property(x => x.StartAt).IsRequired();
            builder.Property(x => x.EndAt).IsRequired();
            builder.Property(x => x.ExpectedReturnAt).IsRequired();

            builder.Property(x => x.DailyPriceInCents).IsRequired();
            builder.Property(x => x.PriceInCents).IsRequired();
            builder.Property(x => x.PenaltyPriceInCents);


            builder
                .HasOne(r => r.Motorcycle)
                .WithMany(m => m.Rents)
                .HasForeignKey(r => r.MotorcycleId)
                .IsRequired();

            builder
                .HasOne(r => r.Renter)
                .WithMany(r => r.Rents)
                .HasForeignKey(r => r.RenterId)
                .IsRequired();

            base.Configure(builder);
        }
    }
}
