using FCG.Domain.Entities.Games;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FCG.Infrastructure.Data.Configurations.Games
{
    public class LibraryConfiguration : IEntityTypeConfiguration<Library>
    {
        public void Configure(EntityTypeBuilder<Library> builder)
        {
            builder.ToTable("LIBRARY_TB");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                   .HasColumnName("LIBRARY_ID")
                   .HasDefaultValueSql("uuid_generate_v4()");

            builder.Property(x => x.PurchasedAt)
                   .HasColumnName("DT_PURCHASED_AT")
                   .IsRequired();

            builder.Property(x => x.UserId)
                   .HasColumnName("USER_ID");

            builder.Property(x => x.GameId)
                   .HasColumnName("GAME_ID");

            builder.HasIndex(x => new { x.UserId, x.GameId })
                   .IsUnique();
        }
    }
}