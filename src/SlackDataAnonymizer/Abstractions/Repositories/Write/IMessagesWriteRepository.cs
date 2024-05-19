using SlackDataAnonymizer.Models.Slack;

namespace SlackDataAnonymizer.Abstractions.Repositories.Write;

public interface IMessagesWriteRepository
{
    ValueTask CreateSlackMessagesAsync(IAsyncEnumerable<SlackMessage> slackMessages, CancellationToken cancellationToken);
}