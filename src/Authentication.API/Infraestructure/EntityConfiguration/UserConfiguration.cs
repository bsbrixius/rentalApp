using Authentication.API.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authentication.API.Infraestructure.EntityConfiguration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(p => p.FirstName)
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(p => p.LastName)
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(p => p.Birthday)
            .IsRequired();

            builder.HasMany(e => e.Roles)
                .WithMany(e => e.Users)
                .UsingEntity<UserRole>(
                j => j
                    .HasOne<Role>()
                    .WithMany()
                    .HasForeignKey(nameof(UserRole.RoleId)),
                j => j
                    .HasOne<User>()
                    .WithMany()
                    .HasForeignKey(nameof(UserRole.UserId)),
                j =>
                {
                    j.HasKey(x => new { x.UserId, x.RoleId });
                });
        }
    }
}
