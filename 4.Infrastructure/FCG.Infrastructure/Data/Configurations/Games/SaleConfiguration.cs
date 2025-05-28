using FCG.Domain.Entities.Games;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FCG.Infrastructure.Data.Configurations.Games
{

    public class SaleConfiguration : IEntityTypeConfiguration<Sale>
    {

        public void Configure(EntityTypeBuilder<Sale> builder)
        {
            builder.ToTable("SALE_TB");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                   .HasColumnName("SALE_ID")
                   .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(x => x.Name)
                   .HasColumnName("NAME")
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(x => x.Description)
                   .HasColumnName("DESCRIPTION")
                   .HasMaxLength(500);

            builder.Property(x => x.DiscountPercentage)
                   .HasColumnName("DISCOUNT_PERCENTAGE")
                   .IsRequired()
                   .HasColumnType("decimal(5,2)");

            builder.Property(x => x.StartDate)
                   .HasColumnName("DT_START_DATE")
                   .IsRequired();

            builder.Property(x => x.EndDate)
                   .HasColumnName("DT_END_DATE")
                   .IsRequired();

            builder.Property(x => x.IsActive)
                   .HasColumnName("ACTIVE")
                   .IsRequired()
                   .HasDefaultValue(true);
        }

    }

}