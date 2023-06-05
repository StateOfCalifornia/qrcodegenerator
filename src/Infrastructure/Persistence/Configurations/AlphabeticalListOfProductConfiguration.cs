﻿namespace Infrastructure.Persistence.Configurations;

public class AlphabeticalListOfProductConfiguration : IEntityTypeConfiguration<AlphabeticalListOfProduct>
{
    public void Configure(EntityTypeBuilder<AlphabeticalListOfProduct> entity)
    {
        entity.HasNoKey();
        entity.ToView("Alphabetical list of products");
        entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
        entity.Property(e => e.CategoryName)
            .IsRequired()
            .HasMaxLength(15);
        entity.Property(e => e.ProductId).HasColumnName("ProductID");
        entity.Property(e => e.ProductName)
            .IsRequired()
            .HasMaxLength(40);
        entity.Property(e => e.QuantityPerUnit).HasMaxLength(20);
        entity.Property(e => e.SupplierId).HasColumnName("SupplierID");
        entity.Property(e => e.UnitPrice).HasColumnType("money");
    }
}
