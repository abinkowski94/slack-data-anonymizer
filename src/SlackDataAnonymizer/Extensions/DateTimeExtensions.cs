namespace SlackDataAnonymizer.Extensions;

public static class DateTimeExtensions
{
    public static DateTime UnixTimeStampToUtcDateTime(this double unixTimeStamp)
    {
        var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp).ToUniversalTime();

        return dateTime;
    }

    public static int GetYearQuarter(this DateTime date)
    {
        return date.Month switch
        {
            >= 1 and <= 3 => 1,
            >=4 and <= 6 => 2,
            >= 7 and <= 9 => 3,
            _ => 4,
        };
    }

    public static int GetYearSemester(this DateTime date)
    {
        return date.Month <= 6 ? 1 : 2;
    }
}
