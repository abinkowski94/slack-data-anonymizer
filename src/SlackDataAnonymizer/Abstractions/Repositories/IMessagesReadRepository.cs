using SlackDataAnonymizer.Models.Slack;

namespace SlackDataAnonymizer.Abstractions.Repositories;

public interface IMessagesReadRepository
{
    IAsyncEnumerable<SlackMessage> GetSlackMessagesAsync(CancellationToken cancellationToken);
}