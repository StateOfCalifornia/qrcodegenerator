namespace Infrastructure.Persistence.Configurations;

public class CategorySalesFor1997Configuration : IEntityTypeConfiguration<CategorySalesFor1997>
{
    public void Configure(EntityTypeBuilder<CategorySalesFor1997> entity)
    {
        entity.HasNoKey();

        entity.ToView("Category Sales for 1997");

        entity.Property(e => e.CategoryName)
            .IsRequired()
            .HasMaxLength(15);

        entity.Property(e => e.CategorySales).HasColumnType("money");
    }
}
