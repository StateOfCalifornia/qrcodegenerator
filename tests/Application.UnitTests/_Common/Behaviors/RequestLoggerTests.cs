namespace Application.UnitTests.Common.Behaviors;

public class RequestLoggerTests
{
    private readonly Mock<ILogger<GetAllCustomersQuery>> _logger = null!;
    private readonly Mock<ICurrentUserService> _currentUserService = null!;

    #region Constructor
    public RequestLoggerTests()
    {
        _logger = new Mock<ILogger<GetAllCustomersQuery>>();
        _currentUserService = new Mock<ICurrentUserService>();
    }
    #endregion

    //[Fact]
    //public async Task ShouldCallGetUserNameAsyncOnceIfAuthenticated()
    //{
    //    _currentUserService.Setup(x => x.UserId).Returns(Guid.NewGuid().ToString());

    //    var requestLogger = new LoggingBehavior<GetAllQuery>(_logger.Object, _currentUserService.Object);

    //    await requestLogger.Process(new GetAllQuery { }, new CancellationToken());

    //    //_identityService.Verify(i => i.GetUserNameAsync(It.IsAny<string>()), Times.Once);
    //    _logger.Verify(i => i.LogInformation(It.IsAny<string>()), Times.Once);
    //}

}
