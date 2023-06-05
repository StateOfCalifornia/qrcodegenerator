namespace Infrastructure.Persistence.Configurations;

public class SummaryOfSalesByQuarterConfiguration : IEntityTypeConfiguration<SummaryOfSalesByQuarter>
{
    public void Configure(EntityTypeBuilder<SummaryOfSalesByQuarter> entity)
    {
        entity.HasNoKey();

        entity.ToView("Summary of Sales by Quarter");

        entity.Property(e => e.OrderId).HasColumnName("OrderID");

        entity.Property(e => e.ShippedDate).HasColumnType("datetime");

        entity.Property(e => e.Subtotal).HasColumnType("money");
    }
}
