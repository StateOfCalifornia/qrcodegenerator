namespace Infrastructure.Persistence.Configurations;

public class SalesTotalsByAmountConfiguration : IEntityTypeConfiguration<SalesTotalsByAmount>
{
    public void Configure(EntityTypeBuilder<SalesTotalsByAmount> entity)
    {
        entity.HasNoKey();

        entity.ToView("Sales Totals by Amount");

        entity.Property(e => e.CompanyName)
            .IsRequired()
            .HasMaxLength(40);

        entity.Property(e => e.OrderId).HasColumnName("OrderID");

        entity.Property(e => e.SaleAmount).HasColumnType("money");

        entity.Property(e => e.ShippedDate).HasColumnType("datetime");
    }
}
