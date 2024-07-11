using SlackDataAnonymizer.Abstractions.Service;
using SlackDataAnonymizer.Commands;

namespace SlackDataAnonymizer.Abstractions.Factories;

public interface IMessagesServiceFactory
{
    IMessagesService Create(AnonymizeConsoleCommand consoleCommand);
}