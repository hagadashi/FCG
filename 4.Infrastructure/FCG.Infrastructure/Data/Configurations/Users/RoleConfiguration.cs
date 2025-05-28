using FCG.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FCG.Infrastructure.Data.Configurations.Users
{

    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("ROLE_TB");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                   .HasColumnName("ROLE_ID")
                   .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(x => x.Name)
                   .HasColumnName("NAME")
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(x => x.Description)
                   .HasColumnName("DESCRIPTION")
                   .HasMaxLength(200);

            builder.HasIndex(x => x.Name)
                .IsUnique();
        }
    }

}