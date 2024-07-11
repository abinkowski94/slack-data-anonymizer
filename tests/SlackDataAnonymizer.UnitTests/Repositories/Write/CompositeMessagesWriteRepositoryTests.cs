using SlackDataAnonymizer.Models.Enums;
using SlackDataAnonymizer.Models.Slack;
using SlackDataAnonymizer.Repositories.Write;
using SlackDataAnonymizer.Serialization;
using System.Text.Json;

namespace SlackDataAnonymizer.UnitTests.Repositories.Write;

public class CompositeMessagesWriteRepositoryTests : IDisposable
{
    private readonly JsonSerializerOptions options;
    private readonly string targetDirectory;

    public CompositeMessagesWriteRepositoryTests()
    {
        options = SerializationConsts.Options;
        targetDirectory = Path.Combine(Directory.GetCurrentDirectory(), @"SlackDataAnonymizer\UnitTests\CompositeMessagesWriteRepositoryTests");

        Directory.CreateDirectory(targetDirectory);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        Directory.Delete(targetDirectory, true);
    }

    [Fact]
    public async Task CreateSlackMessagesAsync_CreatesDailyFilesCorrectly()
    {
        // Arrange
        var aggregationMode = AggregationMode.Daily;
        var sut = new CompositeMessagesWriteRepository(options, targetDirectory, aggregationMode);
        var slackMessages = ToAsyncEnumerable(GetTestSlackMessages());

        // Act
        await sut.CreateSlackMessagesAsync(slackMessages, CancellationToken.None);

        // Assert
        var files = Directory.GetFiles(targetDirectory);
        Assert.NotEmpty(files);
        Assert.All(files, file => Assert.EndsWith(".json", file));
        Assert.Contains(files, file => file.EndsWith("2021-06-01.json"));
        Assert.Contains(files, file => file.EndsWith("2021-06-02.json"));
        Assert.Contains(files, file => file.EndsWith("2021-06-03.json"));
        Assert.Contains(files, file => file.EndsWith("2022-07-01.json"));
        Assert.Contains(files, file => file.EndsWith("2023-08-01.json"));
    }

    [Fact]
    public async Task CreateSlackMessagesAsync_CreatesMonthlyFilesCorrectly()
    {
        // Arrange
        var aggregationMode = AggregationMode.Monthly;
        var sut = new CompositeMessagesWriteRepository(options, targetDirectory, aggregationMode);
        var slackMessages = ToAsyncEnumerable(GetTestSlackMessages());

        // Act
        await sut.CreateSlackMessagesAsync(slackMessages, CancellationToken.None);

        // Assert
        var files = Directory.GetFiles(targetDirectory);
        Assert.NotEmpty(files);
        Assert.All(files, file => Assert.EndsWith(".json", file));
        Assert.Contains(files, file => file.EndsWith("2021-06.json"));
        Assert.Contains(files, file => file.EndsWith("2022-07.json"));
        Assert.Contains(files, file => file.EndsWith("2023-08.json"));
    }

    [Fact]
    public async Task CreateSlackMessagesAsync_CreatesQuarterlyFilesCorrectly()
    {
        // Arrange
        var aggregationMode = AggregationMode.Quarterly;
        var sut = new CompositeMessagesWriteRepository(options, targetDirectory, aggregationMode);
        var slackMessages = ToAsyncEnumerable(GetTestSlackMessages());

        // Act
        await sut.CreateSlackMessagesAsync(slackMessages, CancellationToken.None);

        // Assert
        var files = Directory.GetFiles(targetDirectory);
        Assert.NotEmpty(files);
        Assert.All(files, file => Assert.EndsWith(".json", file));
        Assert.Contains(files, file => file.EndsWith("2021-Q2.json"));
        Assert.Contains(files, file => file.EndsWith("2022-Q3.json"));
        Assert.Contains(files, file => file.EndsWith("2023-Q3.json"));
    }

    [Fact]
    public async Task CreateSlackMessagesAsync_CreatesSemestralyFilesCorrectly()
    {
        // Arrange
        var aggregationMode = AggregationMode.Semestraly;
        var sut = new CompositeMessagesWriteRepository(options, targetDirectory, aggregationMode);
        var slackMessages = ToAsyncEnumerable(GetTestSlackMessages());

        // Act
        await sut.CreateSlackMessagesAsync(slackMessages, CancellationToken.None);

        // Assert
        var files = Directory.GetFiles(targetDirectory);
        Assert.NotEmpty(files);
        Assert.All(files, file => Assert.EndsWith(".json", file));
        Assert.Contains(files, file => file.EndsWith("2021-S1.json"));
        Assert.Contains(files, file => file.EndsWith("2022-S2.json"));
        Assert.Contains(files, file => file.EndsWith("2023-S2.json"));
    }

    [Fact]
    public async Task CreateSlackMessagesAsync_CreatesYearlyFilesCorrectly()
    {
        // Arrange
        var aggregationMode = AggregationMode.Yearly;
        var sut = new CompositeMessagesWriteRepository(options, targetDirectory, aggregationMode);
        var slackMessages = ToAsyncEnumerable(GetTestSlackMessages());

        // Act
        await sut.CreateSlackMessagesAsync(slackMessages, CancellationToken.None);

        // Assert
        var files = Directory.GetFiles(targetDirectory);
        Assert.NotEmpty(files);
        Assert.All(files, file => Assert.EndsWith(".json", file));
        Assert.Contains(files, file => file.EndsWith("2021.json"));
        Assert.Contains(files, file => file.EndsWith("2022.json"));
        Assert.Contains(files, file => file.EndsWith("2023.json"));
    }

    private static IEnumerable<SlackMessage> GetTestSlackMessages()
    {
        return
        [
            new SlackMessage
            {
                TimeStamp = ToUnixTimeSecondsString(2021, 06, 01),
                Text = "Message 1"
            },
            new SlackMessage
            {
                TimeStamp = ToUnixTimeSecondsString(2021, 06, 02),
                Text = "Message 2"
            },
            new SlackMessage
            {
                TimeStamp = ToUnixTimeSecondsString(2021, 06, 03),
                Text = "Message 3"
            },
            new SlackMessage
            {
                TimeStamp = ToUnixTimeSecondsString(2022, 07, 01),
                Text = "Message 4"
            },
            new SlackMessage
            {
                TimeStamp = ToUnixTimeSecondsString(2023, 08, 01),
                Text = "Message 5"
            },
        ];
    }

    private static string ToUnixTimeSecondsString(int year, int month, int day)
    {
        var date = new DateTime(year, month, day, 12, 00, 00);
        var offset = TimeSpan.Zero;

        return new DateTimeOffset(date, offset)
            .ToUnixTimeSeconds()
            .ToString();
    }

    private static async IAsyncEnumerable<SlackMessage> ToAsyncEnumerable(IEnumerable<SlackMessage> slackMessages)
    {
        foreach (var slackMessage in slackMessages)
        {
            yield return slackMessage;
            await Task.Yield();
        }
    }
}
