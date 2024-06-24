﻿using BuildingBlocks.Infrastructure.Configuration;
using Core.API.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Infrastructure.EntityConfiguration
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
