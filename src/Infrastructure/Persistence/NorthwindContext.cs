namespace Infrastructure.Persistence;

public class NorthwindContext : DbContext, INorthwindContext
{
    private const string EVENT_SCHEDULER_NAME = "EVENT-SCHEDULER";

    #region Private DI readonly properties
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTimeService _dateTime;
    #endregion

    #region Constructors
    public NorthwindContext(DbContextOptions<NorthwindContext> options) : base(options) { }

    public NorthwindContext(DbContextOptions<NorthwindContext> options, ICurrentUserService currentUserService, IDateTimeService dateTime) : base(options)
    {
        _currentUserService = currentUserService;
        _dateTime = dateTime;
    }
    #endregion

    #region DbSets
    public DbSet<AlphabeticalListOfProduct> AlphabeticalListOfProducts { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<CategorySalesFor1997> CategorySalesFor1997s { get; set; }
    public DbSet<CurrentProductList> CurrentProductLists { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<CustomerAndSuppliersByCity> CustomerAndSuppliersByCities { get; set; }
    public DbSet<CustomerDemographic> CustomerDemographics { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderDetail> OrderDetails { get; set; }
    public DbSet<OrderDetailsExtended> OrderDetailsExtendeds { get; set; }
    public DbSet<OrderSubtotal> OrderSubtotals { get; set; }
    public DbSet<OrdersQry> OrdersQries { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductSalesFor1997> ProductSalesFor1997s { get; set; }
    public DbSet<ProductsAboveAveragePrice> ProductsAboveAveragePrices { get; set; }
    public DbSet<ProductsByCategory> ProductsByCategories { get; set; }
    public DbSet<QuarterlyOrder> QuarterlyOrders { get; set; }
    public DbSet<Region> Regions { get; set; }
    public DbSet<SalesByCategory> SalesByCategories { get; set; }
    public DbSet<SalesTotalsByAmount> SalesTotalsByAmounts { get; set; }
    public DbSet<Shipper> Shippers { get; set; }
    public DbSet<SummaryOfSalesByQuarter> SummaryOfSalesByQuarters { get; set; }
    public DbSet<SummaryOfSalesByYear> SummaryOfSalesByYears { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<Territory> Territories { get; set; }
    #endregion

    #region INorthwindContext Implementation
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = _currentUserService.IsAuthenticated ? _currentUserService.UserId : EVENT_SCHEDULER_NAME;
                    entry.Entity.CreatedDate = _dateTime.Now;
                    break;
                case EntityState.Modified:
                    entry.Entity.ModifiedBy = _currentUserService.IsAuthenticated ? _currentUserService.UserId : EVENT_SCHEDULER_NAME;
                    entry.Entity.ModifiedDate = _dateTime.Now;
                    break;
            }
        }

        var result = await base.SaveChangesAsync(cancellationToken);
        return result;
    }
    #endregion

    #region DbContext Overrides
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
    #endregion
}
