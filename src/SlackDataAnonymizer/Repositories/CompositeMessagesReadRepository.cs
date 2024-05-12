using SlackDataAnonymizer.Abstractions.Repositories;
using SlackDataAnonymizer.Models.Slack;
using System.Runtime.CompilerServices;

namespace SlackDataAnonymizer.Repositories;

public class CompositeMessagesReadRepository(
    IEnumerable<IMessagesReadRepository> readRepositories) : IMessagesReadRepository
{
    private readonly IEnumerable<IMessagesReadRepository> readRepositories = readRepositories;

    public async IAsyncEnumerable<SlackMessage> GetSlackMessagesAsync([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        foreach (var readRepository in readRepositories)
        {
            await foreach (var message in readRepository.GetSlackMessagesAsync(cancellationToken))
            {
                yield return message;
            }
        }
    }
}
