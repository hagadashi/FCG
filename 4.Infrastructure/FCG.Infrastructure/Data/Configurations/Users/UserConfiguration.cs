using FCG.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FCG.Infrastructure.Data.Configurations.Users
{

    public class UserConfiguration : IEntityTypeConfiguration<User>
    {

        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("USER_TB");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                   .HasColumnName("USER_ID")
                   .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(x => x.Username)
                   .HasColumnName("USERNAME")
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(x => x.Email)
                   .HasColumnName("EMAIL")
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(x => x.PasswordHash)
                   .HasColumnName("PASSWORD")
                   .IsRequired();

            builder.Property(x => x.FirstName)
                   .HasColumnName("FIRSTNAME")
                   .HasMaxLength(100);

            builder.Property(x => x.LastName)
                   .HasColumnName("LASTNAME")
                   .HasMaxLength(100);

            builder.Property(x => x.IsActive)
                   .HasColumnName("ACTIVE")
                   .IsRequired()
                   .HasDefaultValue(true);

            builder.HasIndex(x => x.Username)
                   .IsUnique();

            builder.HasIndex(x => x.Email)
                   .IsUnique();
        }
    }

}