using SlackDataAnonymizer.Abstractions.Repositories.Write;
using SlackDataAnonymizer.Models.Enums;

namespace SlackDataAnonymizer.Abstractions.Factories;

public interface IMessagesWriteRepositoryFactory
{
    IMessagesWriteRepository Create(string targetDirectory, AggregationMode aggregationMode);
}
