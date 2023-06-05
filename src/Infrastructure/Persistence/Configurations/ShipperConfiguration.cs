namespace Infrastructure.Persistence.Configurations;

public class ShipperConfiguration : IEntityTypeConfiguration<Shipper>
{
    public void Configure(EntityTypeBuilder<Shipper> entity)
    {
        entity.Property(e => e.ShipperId).HasColumnName("ShipperID");

        entity.Property(e => e.CompanyName)
            .IsRequired()
            .HasMaxLength(40);

        entity.Property(e => e.Phone).HasMaxLength(24);
    }
}
