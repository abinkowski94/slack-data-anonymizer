using SlackDataAnonymizer.Abstractions.Repositories.Write;
using SlackDataAnonymizer.Models.Slack;
using System.Text.Json;

namespace SlackDataAnonymizer.Repositories.Write;

public class MessagesWriteRepository(
    string messagesFilePath,
    JsonSerializerOptions? options = null)
    : WriteRepositoryBase, IMessagesWriteRepository
{
    private readonly string messagesFilePath = messagesFilePath;
    private readonly JsonSerializerOptions options = options ?? new();

    public async ValueTask CreateSlackMessagesAsync(IAsyncEnumerable<SlackMessage> slackMessages, CancellationToken cancellationToken)
    {
        EnsuerDirectoryExists(messagesFilePath);

        await using var fileStream = new FileStream(messagesFilePath, FileMode.OpenOrCreate, FileAccess.Write);

        await JsonSerializer.SerializeAsync(fileStream, slackMessages, options, cancellationToken);
    }
}