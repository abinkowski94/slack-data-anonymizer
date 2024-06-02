using SlackDataAnonymizer.Abstractions.Factories;
using SlackDataAnonymizer.Abstractions.Repositories.Write;
using SlackDataAnonymizer.Models.Enums;
using SlackDataAnonymizer.Repositories.Write;
using System.Text.Json;

namespace SlackDataAnonymizer.Factories;

public class MessagesWriteRepositoryFactory(JsonSerializerOptions options) : IMessagesWriteRepositoryFactory
{
    private readonly JsonSerializerOptions options = options;

    public IMessagesWriteRepository Create(string targetDirectory, AggregationMode aggregationMode)
    {
        return new CompositeMessagesWriteRepository(options, targetDirectory, aggregationMode);
    }
}
