namespace Infrastructure.Persistence.Configurations;

public class TerritoryConfiguration : IEntityTypeConfiguration<Territory>
{
    public void Configure(EntityTypeBuilder<Territory> entity)
    {
        entity.HasKey(e => e.TerritoryId)
            .IsClustered(false);

        entity.Property(e => e.TerritoryId)
            .HasMaxLength(20)
            .HasColumnName("TerritoryID");

        entity.Property(e => e.RegionId).HasColumnName("RegionID");

        entity.Property(e => e.TerritoryDescription)
            .IsRequired()
            .HasMaxLength(50)
            .IsFixedLength();

        entity.HasOne(d => d.Region)
            .WithMany(p => p.Territories)
            .HasForeignKey(d => d.RegionId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Territories_Region");
    }
}
