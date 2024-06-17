using Authentication.API.Domain;
using BuildingBlocks.Security.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authentication.API.Infraestructure.EntityConfiguration
{
    internal class UserBaseConfiguration : IEntityTypeConfiguration<UserBase>
    {
        public void Configure(EntityTypeBuilder<UserBase> builder)
        {
            builder.HasMany(e => e.Roles)
                .WithMany(e => e.Users)
                .UsingEntity<UserRole>(
                j => j
                    .HasOne<Role>()
                    .WithMany()
                    .HasForeignKey(nameof(UserRole.RoleId)),
                j => j
                    .HasOne<UserBase>()
                    .WithMany()
                    .HasForeignKey(nameof(UserRole.UserId)),
                j =>
                {
                    j.HasKey(x => new { x.UserId, x.RoleId });
                });
        }
    }
}
