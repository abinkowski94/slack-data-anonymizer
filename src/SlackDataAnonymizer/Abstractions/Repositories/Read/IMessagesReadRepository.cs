using SlackDataAnonymizer.Models.Slack;

namespace SlackDataAnonymizer.Abstractions.Repositories.Read;

public interface IMessagesReadRepository
{
    IAsyncEnumerable<SlackMessage> GetSlackMessagesAsync(CancellationToken cancellationToken);
}