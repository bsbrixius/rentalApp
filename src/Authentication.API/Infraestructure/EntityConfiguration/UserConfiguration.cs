using Authentication.Domain.Domain;
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
        }
    }
}
