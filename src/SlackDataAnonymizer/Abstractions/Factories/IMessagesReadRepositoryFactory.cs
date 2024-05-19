using SlackDataAnonymizer.Abstractions.Repositories.Read;

namespace SlackDataAnonymizer.Abstractions.Factories;

public interface IMessagesReadRepositoryFactory
{
    IMessagesReadRepository Create(string sourceDirectory);
}