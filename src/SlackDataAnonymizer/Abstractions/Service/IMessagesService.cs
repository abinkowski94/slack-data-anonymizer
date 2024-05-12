namespace SlackDataAnonymizer.Abstractions.Service;

public interface IMessagesService
{
    ValueTask AnonymizeMessagesAsync(CancellationToken cancellationToken);
}