namespace Infrastructure.Persistence.Configurations;

public class SalesByCategoryConfiguration : IEntityTypeConfiguration<SalesByCategory>
{
    public void Configure(EntityTypeBuilder<SalesByCategory> entity)
    {
        entity.HasNoKey();

        entity.ToView("Sales by Category");

        entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

        entity.Property(e => e.CategoryName)
            .IsRequired()
            .HasMaxLength(15);

        entity.Property(e => e.ProductName)
            .IsRequired()
            .HasMaxLength(40);

        entity.Property(e => e.ProductSales).HasColumnType("money");
    }
}
