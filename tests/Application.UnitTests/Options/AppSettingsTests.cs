using ValidationException =  System.ComponentModel.DataAnnotations.ValidationException;
namespace Application.UnitTests.Options;

public class AppSettingsTests
{
    private readonly AppSettings _appSettings;

    #region Constructor
    public AppSettingsTests()
    {
        _appSettings = new AppSettings
        {
            CacheingInMinutesSample = 2, 
            WebUrlSample = "https://myurl.com",
            PerformanceThresholdInMilliseconds = 1001
        };
    }
    #endregion

    #region CacheInMinutes Validation
    [Theory]
    [InlineData(1440)]
    [InlineData(1)]
    public void Validate_CacheInMinutesSample_ShouldBeValid(int minutes)
    {
        //Arrange
        _appSettings.CacheingInMinutesSample = minutes;
        //Act
        _appSettings.Invoking(x => x.Validate()).Should().NotThrow<ValidationException>();
    }

    [Theory]
    [InlineData(2000)]
    [InlineData(1441)]
    [InlineData(0)]
    public void Validate_CacheInMinutesSample_ShouldThrowValidationException(int minutes)
    {
        //Arrange
        var appSettings = new AppSettings { CacheingInMinutesSample = minutes };
        //Act
        appSettings.Invoking(x => x.Validate()).Should().Throw<ValidationException>();
    }
    #endregion

    #region WebUrl Validation
    [Theory]
    [InlineData("https://mywebsite.com")]
    [InlineData("http://mywebsite.com")]
    [InlineData("http://www.mywebsite.com")]
    [InlineData("https://www.mywebsite.com")]
    public void Validate_WebUrl_ShouldBeValid(string webUrl)
    {
        //Arrange
        _appSettings.WebUrlSample = webUrl;
        //Act
        _appSettings.Invoking(x => x.Validate()).Should().NotThrow<ValidationException>();
    }

    [Theory]
    [InlineData("")]
    [InlineData("asdfsadf")]
    [InlineData("  ")]
    [InlineData("asfsf.com")]
    public void Validate_WebUrl_ShouldThrowValidationException(string webUrl)
    {
        //Arrange
        _appSettings.WebUrlSample = webUrl;
        //Act
        _appSettings.Invoking(x => x.Validate()).Should().Throw<ValidationException>();
    }
    #endregion

    #region PerformanceThresholdInMilliseconds Validation
    [Theory]
    [InlineData(1000)]
    [InlineData(10000)]
    public void Validate_PerformanceThresholdInMilliseconds_ShouldBeValid(int performanceThresholdInMilliseconds)
    {
        //Arrange
        _appSettings.PerformanceThresholdInMilliseconds = performanceThresholdInMilliseconds;
        //Act
        _appSettings.Invoking(x => x.Validate()).Should().NotThrow<ValidationException>();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(999)]
    [InlineData(10001)]
    [InlineData(-1000)]
    public void Validate_PerformanceThresholdInMilliseconds_ShouldThrowValidationException(int performanceThresholdInMilliseconds)
    {
        //Arrange
        _appSettings.PerformanceThresholdInMilliseconds = performanceThresholdInMilliseconds;
        //Act
        _appSettings.Invoking(x => x.Validate()).Should().Throw<ValidationException>();
    }
    #endregion
}