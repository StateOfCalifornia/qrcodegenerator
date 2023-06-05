namespace Application.UnitTests.Common.Mappings;

public class MappingTests
{
    private readonly IConfigurationProvider _configuration;
    private readonly IMapper _mapper;

    #region Constructor
    public MappingTests()
    {
        _configuration = new MapperConfiguration(config => config.AddProfile<MappingProfile>());
        _mapper = _configuration.CreateMapper();
    }
    #endregion

    #region Tests

    [Fact]
    public void ShouldHaveValidConfiguration()
    {
        _configuration.AssertConfigurationIsValid();
    }

    /// <summary>Checks if source and destination properly map. </summary>
    /// <remarks>As you build out and map your domain and view models, add their mappings here to test</remarks>
    [Theory]
    [InlineData(typeof(Customer), typeof(CustomerViewModel))]
    public void ShouldSupportMappingFromSourceToDestination(Type source, Type destination)
    {
        var instance = GetInstanceOf(source);
        _mapper.Map(instance, source, destination);
    }

    #endregion

    #region Private Methods
    private static object GetInstanceOf(Type type)
    {
        if (type.GetConstructor(Type.EmptyTypes) != null)
            return Activator.CreateInstance(type)!;

        // Type without parameterless constructor
        return FormatterServices.GetUninitializedObject(type);
    }
    #endregion
}
