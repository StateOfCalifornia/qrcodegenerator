namespace Infrastructure.Persistence.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> entity)
    {
        entity.HasIndex(e => e.City, "City");

        entity.HasIndex(e => e.CompanyName, "CompanyName");

        entity.HasIndex(e => e.PostalCode, "PostalCode");

        entity.HasIndex(e => e.Region, "Region");

        entity.Property(e => e.CustomerId)
            .HasMaxLength(5)
            .HasColumnName("CustomerID")
            .IsFixedLength();

        entity.Property(e => e.Address).HasMaxLength(60);

        entity.Property(e => e.City).HasMaxLength(15);

        entity.Property(e => e.CompanyName)
            .IsRequired()
            .HasMaxLength(40);

        entity.Property(e => e.ContactName).HasMaxLength(30);

        entity.Property(e => e.ContactTitle).HasMaxLength(30);

        entity.Property(e => e.Country).HasMaxLength(15);

        entity.Property(e => e.Fax).HasMaxLength(24);

        entity.Property(e => e.Phone).HasMaxLength(24);

        entity.Property(e => e.PostalCode).HasMaxLength(10);

        entity.Property(e => e.Region).HasMaxLength(15);

        entity.HasMany(d => d.CustomerTypes)
            .WithMany(p => p.Customers)
            .UsingEntity<Dictionary<string, object>>(
                "CustomerCustomerDemo",
                l => l.HasOne<CustomerDemographic>().WithMany().HasForeignKey("CustomerTypeId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_CustomerCustomerDemo"),
                r => r.HasOne<Customer>().WithMany().HasForeignKey("CustomerId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_CustomerCustomerDemo_Customers"),
                j =>
                {
                    j.HasKey("CustomerId", "CustomerTypeId").IsClustered(false);

                    j.ToTable("CustomerCustomerDemo");

                    j.IndexerProperty<string>("CustomerId").HasMaxLength(5).HasColumnName("CustomerID").IsFixedLength();

                    j.IndexerProperty<string>("CustomerTypeId").HasMaxLength(10).HasColumnName("CustomerTypeID").IsFixedLength();
                });
    }
}
