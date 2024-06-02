using SlackDataAnonymizer.Commands;
using SlackDataAnonymizer.Services;

namespace SlackDataAnonymizer.Abstractions.Factories;
public interface IMessagesServiceFactory
{
    MessagesService Create(AnonymizeConsoleCommand consoleCommand);
}