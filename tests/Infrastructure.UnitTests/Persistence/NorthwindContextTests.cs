namespace Infrastructure.UnitTests.Persistence;

public class NorthwindContextTests
{
    private readonly Customer _customer;
    private readonly NorthwindContext _sut;

    #region Constructor
    public NorthwindContextTests()
    {
        // Setup Context
        _sut = NorthwindContextFactory.Create();

        // Setup new customer
        var newCustomer = new Customer
        {
            CustomerId = "DISNEY",
            CompanyName = "Disney Corporation",
            ContactName = "Walt Disney",
            ContactTitle = "Broomstick Manager",
            Address = "123 Lullaby Lane",
            City = "Neverland",
            Region = null,
            PostalCode = "95123",
            Country = "USA",
            Phone = "00-85-4587",
            Fax = "11-85-4444"
        };
        _customer = newCustomer;
    }
    #endregion


    #region Tests

    [Fact]
    public async Task SaveChangesAsync_GivenNewRequest_ShouldSetCreatedProperties()
    {
        //Arrange
        _sut.Customers.Add(_customer);

        //Act        
        await _sut.SaveChangesAsync();

        //Assert
        _customer.CreatedBy.Should().Be(NorthwindContextFactory.DefaultTestUserID);
        _customer.CreatedDate.Should().Be(NorthwindContextFactory.DefaultTestDateTime);
        _customer.ModifiedBy.Should().BeNull();
        _customer.ModifiedDate.Should().BeNull();
    }

    [Fact]
    public async Task SaveChangesAsync_GivenExistingRequest_ShouldSetLastModifiedProperties()
    {
        //Arrange
        _sut.Customers.Add(_customer);
        await _sut.SaveChangesAsync();
        _customer.ContactTitle = "New Contact Title";

        //Act
        _sut.Customers.Update(_customer);
        await _sut.SaveChangesAsync();

        //Assert
        _customer.CreatedBy.Should().Be(NorthwindContextFactory.DefaultTestUserID);
        _customer.CreatedDate.Should().Be(NorthwindContextFactory.DefaultTestDateTime);
        _customer.ModifiedBy.Should().Be(NorthwindContextFactory.DefaultTestUserID);
        _customer.ModifiedDate.Should().Be(NorthwindContextFactory.DefaultTestDateTime);
    }

    [Fact]
    public async Task SaveChangesAsync_GivenNewRequest_NotAuthenticated_ShouldSetCreatedPropertiesToScheduler()
    {
        //Arrange
        var mockDateTimeService = new Mock<IDateTimeService>();
        mockDateTimeService.Setup(m => m.Now).Returns(NorthwindContextFactory.DefaultTestDateTime);
        var mockCurrentUserService = new Mock<ICurrentUserService>();
        mockCurrentUserService.Setup(m => m.UserId).Returns(string.Empty);
        mockCurrentUserService.Setup(m => m.IsAuthenticated).Returns(false);
        var options = new DbContextOptionsBuilder<NorthwindContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        var sut = new NorthwindContext(options, mockCurrentUserService.Object, mockDateTimeService.Object);
        sut.Customers.Add(_customer);

        //Act
        await sut.SaveChangesAsync();

        //Assert
        _customer.CreatedBy.Should().Be("EVENT-SCHEDULER");
        _customer.CreatedDate.Should().Be(NorthwindContextFactory.DefaultTestDateTime);
        _customer.ModifiedBy.Should().BeNull();
        _customer.ModifiedDate.Should().BeNull();
    }
    #endregion
}
