using SlackDataAnonymizer.Abstractions.Repositories;
using SlackDataAnonymizer.Models.Slack;
using System.Text.Json;

namespace SlackDataAnonymizer.Repositories;

public class MessagesWriteRepository(
    string messagesFilePath,
    JsonSerializerOptions? options = null) : IMessagesWriteRepository
{
    private readonly string messagesFilePath = messagesFilePath;
    private readonly JsonSerializerOptions options = options ?? new();

    public async ValueTask CreateSlackMessagesAsync(IAsyncEnumerable<SlackMessage> slackMessages, CancellationToken cancellationToken)
    {
        await using var fileStream = new FileStream(messagesFilePath, FileMode.OpenOrCreate, FileAccess.Write);

        await JsonSerializer.SerializeAsync(fileStream, slackMessages, options, cancellationToken);
    }
}