using SlackDataAnonymizer.Abstractions.Repositories.Read;
using SlackDataAnonymizer.Exceptions;
using SlackDataAnonymizer.Models.Slack;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace SlackDataAnonymizer.Repositories.Read;

public class MessagesReadRepository(
    string messagesPath,
    JsonSerializerOptions? options = null)
    : IMessagesReadRepository
{
    private readonly string messagesPath = messagesPath;
    private readonly JsonSerializerOptions options = options ?? new();

    public async IAsyncEnumerable<SlackMessage> GetSlackMessagesAsync([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        if (!File.Exists(messagesPath))
        {
            throw new FileReadingException(messagesPath);
        }

        if (new FileInfo(messagesPath).Length == 0)
        {
            yield break;
        }

        await using var fileStream = new FileStream(messagesPath, FileMode.Open, FileAccess.Read);

        var messages = JsonSerializer
            .DeserializeAsyncEnumerable<SlackMessage>(fileStream, options, cancellationToken)
            .ConfigureAwait(false);

        await using var enumerator = messages.GetAsyncEnumerator();

        while (await TryGetNextAsync(enumerator))
        {
            if (enumerator.Current is not null)
            {
                yield return enumerator.Current;
            }
        }

        yield break;
    }

    private async Task<bool> TryGetNextAsync(ConfiguredCancelableAsyncEnumerable<SlackMessage?>.Enumerator enumerator)
    {
        try
        {
            return await enumerator.MoveNextAsync();
        }
        catch (Exception ex)
        {
            throw new FileReadingException(messagesPath, ex);
        }
    }
}
