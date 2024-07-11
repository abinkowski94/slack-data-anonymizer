using SlackDataAnonymizer.Extensions;

namespace SlackDataAnonymizer.UnitTests.Extensions;

public class DateTimeExtensionsTests
{
    [Theory]
    [InlineData(0, 1970, 1, 1, 0, 0, 0)]
    [InlineData(1609459200, 2021, 1, 1, 0, 0, 0)] // 2021-01-01 00:00:00 UTC
    [InlineData(1625097600, 2021, 7, 1, 0, 0, 0)] // 2021-07-01 00:00:00 UTC
    public void UnixTimeStampToUtcDateTime_ShouldConvertCorrectly(double unixTimeStamp, int year, int month, int day, int hour, int minute, int second)
    {
        // Act
        var result = unixTimeStamp.UnixTimeStampToUtcDateTime();

        // Assert
        Assert.Equal(year, result.Year);
        Assert.Equal(month, result.Month);
        Assert.Equal(day, result.Day);
        Assert.Equal(hour, result.Hour);
        Assert.Equal(minute, result.Minute);
        Assert.Equal(second, result.Second);
        Assert.Equal(DateTimeKind.Utc, result.Kind);
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(2, 1)]
    [InlineData(3, 1)]
    [InlineData(4, 2)]
    [InlineData(5, 2)]
    [InlineData(6, 2)]
    [InlineData(7, 3)]
    [InlineData(8, 3)]
    [InlineData(9, 3)]
    [InlineData(10, 4)]
    [InlineData(11, 4)]
    [InlineData(12, 4)]
    public void GetYearQuarter_ShouldReturnCorrectQuarter(int month, int expectedQuarter)
    {
        // Arrange
        var date = new DateTime(2021, month, 1);

        // Act
        var result = date.GetYearQuarter();

        // Assert
        Assert.Equal(expectedQuarter, result);
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(2, 1)]
    [InlineData(3, 1)]
    [InlineData(4, 1)]
    [InlineData(5, 1)]
    [InlineData(6, 1)]
    [InlineData(7, 2)]
    [InlineData(8, 2)]
    [InlineData(9, 2)]
    [InlineData(10, 2)]
    [InlineData(11, 2)]
    [InlineData(12, 2)]
    public void GetYearSemester_ShouldReturnCorrectSemester(int month, int expectedSemester)
    {
        // Arrange
        var date = new DateTime(2021, month, 1);

        // Act
        var result = date.GetYearSemester();

        // Assert
        Assert.Equal(expectedSemester, result);
    }
}
