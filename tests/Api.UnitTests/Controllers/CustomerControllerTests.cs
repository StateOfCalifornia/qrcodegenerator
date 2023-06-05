using System.Linq;

namespace Api.UnitTests.Controllers;

[Collection(nameof(ApiUnitTestCollection))]
public class CustomerControllerTests
{
    private readonly CustomerController _sut;
    private readonly int _totalCustomerCount;

    #region Constructor
    public CustomerControllerTests(ApiUnitTestFixture fixture)
    {
        _totalCustomerCount = fixture.IHttpContextAccessorMock.Object.HttpContext.RequestServices.GetService<INorthwindContext>().Customers.Count();
        //Generate Controller
        _sut = new CustomerController(fixture.IHttpContextAccessorMock.Object);
    }
    #endregion

    [Fact]
    public async Task Customers_ShouldReturnOkWithList()
    {
        //Act
        var result = await _sut.Customers();
        
        //Assert
        var objectResult = Assert.IsType<OkObjectResult>(result.Result);
        var model = Assert.IsAssignableFrom<List<CustomerViewModel>>(objectResult.Value);
        objectResult.Should().NotBeNull();
        objectResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        model.Should().BeOfType<List<CustomerViewModel>>();
        model.Count.Should().Be(_totalCustomerCount);
    }
}
