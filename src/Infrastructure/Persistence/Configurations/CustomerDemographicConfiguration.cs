namespace Infrastructure.Persistence.Configurations;

internal class CustomerDemographicConfiguration : IEntityTypeConfiguration<CustomerDemographic>
{
    public void Configure(EntityTypeBuilder<CustomerDemographic> entity)
    {
        entity.HasKey(e => e.CustomerTypeId)
            .IsClustered(false);

        entity.Property(e => e.CustomerTypeId)
            .HasMaxLength(10)
            .HasColumnName("CustomerTypeID")
            .IsFixedLength();

        entity.Property(e => e.CustomerDesc).HasColumnType("ntext");
    }
}
