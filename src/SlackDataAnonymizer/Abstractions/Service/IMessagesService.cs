using SlackDataAnonymizer.Commands;

namespace SlackDataAnonymizer.Abstractions.Service;

public interface IMessagesService
{
    ValueTask AnonymizeMessagesAsync(AnonymizeDataCommand command, CancellationToken cancellationToken);
}