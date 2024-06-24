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
        }
    }
}
