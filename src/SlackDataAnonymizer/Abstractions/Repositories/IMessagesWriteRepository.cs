using SlackDataAnonymizer.Models.Slack;

namespace SlackDataAnonymizer.Abstractions.Repositories;

public interface IMessagesWriteRepository
{
    ValueTask CreateSlackMessagesAsync(IAsyncEnumerable<SlackMessage> slackMessages, CancellationToken cancellationToken);
}