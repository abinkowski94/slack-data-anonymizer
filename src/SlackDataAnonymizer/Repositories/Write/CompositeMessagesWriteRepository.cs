using SlackDataAnonymizer.Abstractions.Repositories.Write;
using SlackDataAnonymizer.Extensions;
using SlackDataAnonymizer.Models.Enums;
using SlackDataAnonymizer.Models.Slack;
using System.Globalization;
using System.Text.Json;
using System.Threading.Channels;

namespace SlackDataAnonymizer.Repositories.Write;

public class CompositeMessagesWriteRepository(
    JsonSerializerOptions options,
    string targetDirectory,
    AggregationMode aggregationMode) : IMessagesWriteRepository
{
    private readonly JsonSerializerOptions options = options;
    private readonly string targetDirectory = targetDirectory;
    private readonly AggregationMode aggregationMode = aggregationMode;

    public async ValueTask CreateSlackMessagesAsync(IAsyncEnumerable<SlackMessage> slackMessages, CancellationToken cancellationToken)
    {
        var currentFilePath = string.Empty;
        var fileStreamChannel = Channel.CreateUnbounded<SlackMessage>();
        var repositoryTask = Task.CompletedTask;

        await foreach (var slackMessage in slackMessages)
        {
            var previousFilePath = currentFilePath;
            currentFilePath = GetFilePath(slackMessage);

            if (previousFilePath != currentFilePath)
            {
                fileStreamChannel.Writer.Complete();
                await repositoryTask;

                fileStreamChannel = Channel.CreateUnbounded<SlackMessage>();

                var repository = new MessagesWriteRepository(currentFilePath, options);
                var messagesStream = fileStreamChannel.Reader.ReadAllAsync(cancellationToken);
                repositoryTask = repository.CreateSlackMessagesAsync(messagesStream, cancellationToken).AsTask();
            }

            await fileStreamChannel.Writer.WriteAsync(slackMessage, cancellationToken);
        }

        fileStreamChannel.Writer.Complete();
        await repositoryTask;
    }

    private string GetFilePath(SlackMessage slackMessage)
    {
        if (double.TryParse(slackMessage.TimeStamp, CultureInfo.InvariantCulture, out var timestamp))
        {
            var date = timestamp.UnixTimeStampToUtcDateTime();

            var fileName = aggregationMode switch
            {
                AggregationMode.Daily => $"{date:yyyy-MM-dd}.json",
                AggregationMode.Monthly => $"{date:yyyy-MM}.json",
                AggregationMode.Quarterly => $"{date:yyyy}-Q{date.GetYearQuarter()}.json",
                AggregationMode.Semestraly => $"{date:yyyy}-S{date.GetYearQuarter()}.json",
                AggregationMode.Yearly => $"{date:yyyy}.json",
                _ => throw new InvalidOperationException($"Aggregation mode {aggregationMode} is not supported."),
            };

            return Path.Combine(targetDirectory, fileName);
        }

        return Path.Combine(targetDirectory, "unkown_timestamp.json");
    }
}
