using Cocona;
using SlackDataAnonymizer.Abstractions.Factories;
using SlackDataAnonymizer.Commands;

namespace SlackDataAnonymizer.Handlers;

public class AnonymizeCommandHandlers(IMessagesServiceFactory messagesServiceFactory)
{
    private readonly IMessagesServiceFactory messagesServiceFactory = messagesServiceFactory;

    [Command("anonymize")]
    public async ValueTask AnonymizeAsync(
        AnonymizeConsoleCommand consoleCommand,
        [Ignore] CancellationToken cancellationToken = default)
    {
        var messageService = messagesServiceFactory.Create(consoleCommand);
        var command = new AnonymizeDataCommand { TextTags = consoleCommand.TextTags.AsReadOnly() };

        await messageService.AnonymizeMessagesAsync(command, cancellationToken).ConfigureAwait(false);
    }
}
