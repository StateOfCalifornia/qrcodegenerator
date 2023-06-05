namespace Infrastructure.UnitTests.Services;

public class DateTimeServiceTests
{
    [Theory]
    [InlineData(2025, 1, 25)]
    [InlineData(1925, 8, 14)]
    [InlineData(1972, 1, 28)]
    [InlineData(1900, 1, 1)]
    public void ShouldPopulateDateWithYearMonthDay(int year, int month, int day)
    {
        //Arrange
        var newDate = new DateTime(year, month, day);

        //Act
        var result = new DateTimeService(newDate);

        //Assert
        result.Now.Should().Be(newDate);
    }

    [Fact]
    public void ShouldPopulateDateWithSystemDateWhenNull()
    {
        var sut = new DateTimeService();
        var result = sut.Now;
        result.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));

    }
}