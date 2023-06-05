namespace Application.UnitTests.Base;

public class NorthwindTestFixture
{
    //Automapper Service References        
    public IMapper Mapper { get; private set; }
    public IConfigurationProvider AutomapperConfigurationProvider { get; }

    //Settings Mocks
    public AppSettings AppSettings { get; private set; }

    //External Services
    public IDateTimeService DateTimeService { get; private set; }
    //public Mock<IRazorViewToStringService> RazorViewToStringRenderer { get; private set; }

    //Northwind
    public NorthwindContext NorthwindContext { get; private set; }

    #region Constructor
    public NorthwindTestFixture()
    {
        // (1) Setup Database
        NorthwindContext = NorthwindContextFactory.Create();

        // (2) Get Automapper
        var configurationProvider = new MapperConfiguration(cfg => { cfg.AddProfile<MappingProfile>(); });
        Mapper = configurationProvider.CreateMapper();

        // (3) Get/Set Services
        //DateTimeService = new IDateTimeService();

        // (3) Base Mock of External Services
        //CurrentUserService = new Mock<ICurrentUserService>();
        //RazorViewToStringRenderer = new Mock<IRazorViewToStringService>();
        //DateTimeService = new Mock<IDateTimeService>();
    }
    #endregion
    
    #region IDisposable Implementation
    public void Dispose()
    {
        NorthwindContextFactory.Destroy(NorthwindContext);
    }
    #endregion

}

[CollectionDefinition(nameof(NorthwindCollection))]
public class NorthwindCollection : ICollectionFixture<NorthwindTestFixture> { }
