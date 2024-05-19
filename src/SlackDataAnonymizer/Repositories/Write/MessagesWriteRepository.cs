using SlackDataAnonymizer.Abstractions.Repositories.Write;
using SlackDataAnonymizer.Models.Slack;
using System.Text.Json;

namespace SlackDataAnonymizer.Repositories.Write;

public class MessagesWriteRepository(
    string messagesFilePath,
    JsonSerializerOptions? options = null)
    : WriteRepositoryBase(messagesFilePath), IMessagesWriteRepository
{
    private readonly JsonSerializerOptions options = options ?? new();

    public async ValueTask CreateSlackMessagesAsync(IAsyncEnumerable<SlackMessage> slackMessages, CancellationToken cancellationToken)
    {
        EnsuerDirectoryExists();

        await using var fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write);

        await JsonSerializer.SerializeAsync(fileStream, slackMessages, options, cancellationToken);
    }
}