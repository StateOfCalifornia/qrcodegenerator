namespace Infrastructure.UnitTests.Services;

public class RazorViewToStringServiceTests
{
    private readonly IRazorViewToStringService _razorViewToStringService;

    #region Constructor
    public RazorViewToStringServiceTests()
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
        var server = new WebApplicationFactory<Startup>();
        _razorViewToStringService = server.Services.CreateScope().ServiceProvider.GetService<IRazorViewToStringService>();
    }
    #endregion

    [Fact]
    public async Task ShouldReturn_SuccessfulView()
    {
        var result = await _razorViewToStringService.RenderViewToStringAsync("/Common/services.cshtml", new ServiceCollection());
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task ShouldThrow_InvalidOperationException()
    {
        Func<Task> func = async () => await _razorViewToStringService.RenderViewToStringAsync("/Common/adsfsadfsaf.cshtml", new ServiceCollection());
        await func.Should().ThrowAsync<InvalidOperationException>();
    }
}