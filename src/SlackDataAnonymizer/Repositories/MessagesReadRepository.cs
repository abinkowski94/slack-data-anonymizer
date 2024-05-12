﻿using SlackDataAnonymizer.Abstractions.Repositories;
using SlackDataAnonymizer.Models.Slack;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace SlackDataAnonymizer.Repositories;

public class MessagesReadRepository(
    string messagesPath,
    JsonSerializerOptions? options = null) : IMessagesReadRepository
{
    private readonly string messagesPath = messagesPath;
    private readonly JsonSerializerOptions options = options ?? new();

    public async IAsyncEnumerable<SlackMessage> GetSlackMessagesAsync([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await using var fileStream = new FileStream(messagesPath, FileMode.Open, FileAccess.Read);

        var messages = JsonSerializer
            .DeserializeAsyncEnumerable<SlackMessage>(fileStream, options, cancellationToken)
            .ConfigureAwait(false);

        await foreach (var message in messages)
        {
            if (message is not null)
            {
                yield return message;
            }
        }

        yield break;
    }
}
