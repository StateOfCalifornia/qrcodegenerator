namespace Application.UnitTests.Customers.Queries;

[Collection(nameof(NorthwindCollection))]
public class GetAllCustomersQueryHandlerTests
{
    private readonly IMapper _mapper;
    private readonly NorthwindContext _context;

    #region Constructor
    public GetAllCustomersQueryHandlerTests(NorthwindTestFixture fixture)
    {
        _mapper = fixture.Mapper;
        _context = fixture.NorthwindContext;
    }
    #endregion

    #region Tests
    [Fact]
    public async Task GetAll_ShouldReturnList_ListShouldBeReturned()
    {
        //Arrange
        var sut = new GetAllCustomersQueryHandler(_context, _mapper);

        //Act
        var result = await sut.Handle(new GetAllCustomersQuery(), CancellationToken.None);

        //Asserts
        result.Should().BeOfType<List<CustomerViewModel>>();
        result.Should().NotBeEmpty();
        result.Count.Should().Be(_context.Customers.ToList().Count);
    }
    #endregion

}
