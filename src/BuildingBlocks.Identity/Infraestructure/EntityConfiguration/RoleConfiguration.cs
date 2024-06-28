using Authentication.API.Domain;
using BuildingBlocks.Security.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authentication.API.Infraestructure.EntityConfiguration
{
    internal class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(r => r.Id);

            // Properties
            builder.Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(128);

            builder.Property(r => r.NormalizedName)
                .IsRequired()
                .HasMaxLength(128);

            builder.HasMany(e => e.Users)
                .WithMany(e => e.Roles)
                .UsingEntity<UserRole>(
                j => j
                    .HasOne<UserBase>()
                    .WithMany()
                    .HasForeignKey(nameof(UserRole.UserId)),
                j => j
                    .HasOne<Role>()
                    .WithMany()
                    .HasForeignKey(nameof(UserRole.RoleId)),
                j =>
                {
                    j.HasKey(x => new { x.UserId, x.RoleId });
                    j.ToTable("UserRole", "identity");
                });

            builder.HasMany(r => r.RoleClaims)
                .WithOne(rc => rc.Role)
                .HasForeignKey(rc => rc.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
