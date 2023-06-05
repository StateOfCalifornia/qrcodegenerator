using FluentValidation;

namespace Api.UnitTests;


public class ApiUnitTestFixture
{
    public Mock<IHttpContextAccessor> IHttpContextAccessorMock { get; private set; }
    
    public ApiUnitTestFixture()
    {
        var appNamespace = nameof(Application);
        var services = new ServiceCollection();
        var context = NorthwindContextFactory.Create();
        services.AddScoped<INorthwindContext>(p => context);
        services.AddAutoMapper(Assembly.Load(appNamespace));
        services.AddValidatorsFromAssembly(Assembly.Load(appNamespace));
        services.AddMediatR(Assembly.Load(appNamespace));

        //Add AppSettings
        //var appSettings = new AppSettings
        //{
        //    CacheingInMinutesSample = 1,
        //    WebUrlSample = "http://test.com",
        //    PerformanceThresholdInMilliseconds = 1000
        //};
        //services.AddSingleton(p => appSettings);
        
        ////Add Common Behaviors To Test
        //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));
        //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehavior<,>));
        //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        services.AddLogging();
        services.AddScoped(p => new Mock<ICurrentUserService>().Object);
        //Get Http Objects
        var defaultHttpContext = new DefaultHttpContext
        {
            RequestServices = services.BuildServiceProvider()
        };

        //Setup mock and add to global
        var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        mockHttpContextAccessor
            .Setup(x => x.HttpContext)
            .Returns(defaultHttpContext);
        IHttpContextAccessorMock = mockHttpContextAccessor;

    }
}

[CollectionDefinition(nameof(ApiUnitTestCollection))]
public class ApiUnitTestCollection : ICollectionFixture<ApiUnitTestFixture> { }