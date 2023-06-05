namespace Infrastructure.Persistence.Configurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> entity)
    {
        entity.HasIndex(e => e.LastName, "LastName");

        entity.HasIndex(e => e.PostalCode, "PostalCode");

        entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

        entity.Property(e => e.Address).HasMaxLength(60);

        entity.Property(e => e.BirthDate).HasColumnType("datetime");

        entity.Property(e => e.City).HasMaxLength(15);

        entity.Property(e => e.Country).HasMaxLength(15);

        entity.Property(e => e.Extension).HasMaxLength(4);

        entity.Property(e => e.FirstName)
            .IsRequired()
            .HasMaxLength(10);

        entity.Property(e => e.HireDate).HasColumnType("datetime");

        entity.Property(e => e.HomePhone).HasMaxLength(24);

        entity.Property(e => e.LastName)
            .IsRequired()
            .HasMaxLength(20);

        entity.Property(e => e.Notes).HasColumnType("ntext");

        entity.Property(e => e.Photo).HasColumnType("image");

        entity.Property(e => e.PhotoPath).HasMaxLength(255);

        entity.Property(e => e.PostalCode).HasMaxLength(10);

        entity.Property(e => e.Region).HasMaxLength(15);

        entity.Property(e => e.Title).HasMaxLength(30);

        entity.Property(e => e.TitleOfCourtesy).HasMaxLength(25);

        entity.HasOne(d => d.ReportsToNavigation)
            .WithMany(p => p.InverseReportsToNavigation)
            .HasForeignKey(d => d.ReportsTo)
            .HasConstraintName("FK_Employees_Employees");

        entity.HasMany(d => d.Territories)
            .WithMany(p => p.Employees)
            .UsingEntity<Dictionary<string, object>>(
                "EmployeeTerritory",
                l => l.HasOne<Territory>().WithMany().HasForeignKey("TerritoryId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_EmployeeTerritories_Territories"),
                r => r.HasOne<Employee>().WithMany().HasForeignKey("EmployeeId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_EmployeeTerritories_Employees"),
                j =>
                {
                    j.HasKey("EmployeeId", "TerritoryId").IsClustered(false);

                    j.ToTable("EmployeeTerritories");

                    j.IndexerProperty<int>("EmployeeId").HasColumnName("EmployeeID");

                    j.IndexerProperty<string>("TerritoryId").HasMaxLength(20).HasColumnName("TerritoryID");
                });
    }
}
