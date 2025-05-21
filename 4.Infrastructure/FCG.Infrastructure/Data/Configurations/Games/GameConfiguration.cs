using FCG.Domain.Entities.Games;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FCG.Infrastructure.Data.Configurations.Games
{

    public class GameConfiguration : IEntityTypeConfiguration<Game>
    {

        public void Configure(EntityTypeBuilder<Game> builder)
        {
            builder.ToTable("GAME_TB");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                   .HasColumnName("GAME_ID")
                   .HasDefaultValueSql("uuid_generate_v4()");

            builder.Property(x => x.Title)
                   .HasColumnName("TITLE")
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(x => x.Description)
                   .HasColumnName("DESCRIPTION")
                   .HasMaxLength(2000);

            builder.Property(x => x.Price)
                   .HasColumnName("PRICE")
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.Property(x => x.ImageUrl)
                   .HasColumnName("IMAGE_URL")
                   .HasMaxLength(500);

            builder.Property(x => x.IsActive)
                   .HasColumnName("ACTIVE")
                   .IsRequired()
                   .HasDefaultValue(true);
        }

    }

}