using SlackDataAnonymizer.Abstractions.Factories;
using SlackDataAnonymizer.Abstractions.Repositories.Read;
using SlackDataAnonymizer.Repositories.Read;
using System.Text.Json;

namespace SlackDataAnonymizer.Factories;

public class MessagesReadRepositoryFactory(JsonSerializerOptions serializerOptions) : IMessagesReadRepositoryFactory
{
    private readonly JsonSerializerOptions serializerOptions = serializerOptions;

    public IMessagesReadRepository Create(string sourceDirectory)
    {
        ArgumentNullException.ThrowIfNull(sourceDirectory, nameof(sourceDirectory));

        var repositories = GetRepositories(sourceDirectory);

        return new CompositeMessagesReadRepository(repositories);
    }

    private List<MessagesReadRepository> GetRepositories(string sourceDirectory)
    {
        if (Directory.Exists(sourceDirectory))
        {
            return Directory
                .EnumerateFiles(sourceDirectory)
                .Select(f => new MessagesReadRepository(f, serializerOptions))
                .ToList();
        }

        return [];
    }
}
