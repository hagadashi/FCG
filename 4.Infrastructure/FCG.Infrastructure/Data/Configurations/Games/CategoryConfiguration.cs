using FCG.Domain.Entities.Games;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FCG.Infrastructure.Data.Configurations.Games
{

    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {

        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("CATEGORY_TB");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                   .HasColumnName("CATEGORY_ID")
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