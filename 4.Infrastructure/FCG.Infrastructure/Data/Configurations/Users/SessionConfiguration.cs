using FCG.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FCG.Infrastructure.Data.Configurations.Users
{

    public class SessionConfiguration : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.ToTable("SESSION_TB");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                   .HasColumnName("SESSION_ID")
                   .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(x => x.Token)
                   .HasColumnName("TOKEN")
                   .IsRequired();

            builder.Property(x => x.ExpiresAt)
                   .HasColumnName("DT_EXPIRES_AT")
                   .IsRequired();

            builder.Property(x => x.IsActive)
                   .HasColumnName("ACTIVE")
                   .IsRequired()
                   .HasDefaultValue(true);

            builder.HasIndex(x => x.Token)
                   .IsUnique();
        }
    }

}