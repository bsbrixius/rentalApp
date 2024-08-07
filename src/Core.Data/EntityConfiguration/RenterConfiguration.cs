﻿using BuildingBlocks.Infrastructure.Configuration;
using Core.Domain.Aggregates.Renter;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Data.EntityConfiguration
{
    public class RenterConfiguration : AuditableEntityTypeConfiguration<Renter>
    {
        public override void Configure(EntityTypeBuilder<Renter> builder)
        {
            builder.HasIndex(builder => builder.CNPJ).IsUnique();
            builder.HasIndex(builder => builder.UserId).IsUnique();

            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.CNPJ).IsRequired();
            builder.Property(x => x.Birthdate).IsRequired();

            builder.HasOne(x => x.CurrentMotorcycle)
                .WithMany()
                .HasForeignKey(x => x.CurrentMotorcycleId)
                .IsRequired(false);

            builder.OwnsOne(x => x.CNH, ownedBuilder =>
            {
                ownedBuilder.HasIndex(y => y.Number).IsUnique(true);
                ownedBuilder.Property(y => y.Number).HasMaxLength(11);
                ownedBuilder.Property(y => y.Type);
                ownedBuilder.Property(y => y.Url);
            });
            base.Configure(builder);
        }
    }
}
