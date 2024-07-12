using SlackDataAnonymizer.Abstractions.Factories;
using SlackDataAnonymizer.Abstractions.Service;
using SlackDataAnonymizer.Commands;
using SlackDataAnonymizer.Services;

namespace SlackDataAnonymizer.Factories;

public class MessagesServiceFactory(
    IMessagesReadRepositoryFactory readRepositoryFactory,
    IMessagesWriteRepositoryFactory writeRepositoryFactory,
    ISensitiveDataWriteRepositoryFactory sensitiveDataWriteRepositoryFactory,
    IAnonymousIdFactory anonymousIdFactory,
    ISlackMessageAnonymizerService anonymizerService)
    : IMessagesServiceFactory
{
    private readonly IMessagesReadRepositoryFactory readRepositoryFactory = readRepositoryFactory;
    private readonly IMessagesWriteRepositoryFactory writeRepositoryFactory = writeRepositoryFactory;
    private readonly ISensitiveDataWriteRepositoryFactory sensitiveDataWriteRepositoryFactory = sensitiveDataWriteRepositoryFactory;
    private readonly IAnonymousIdFactory anonymousIdFactory = anonymousIdFactory;
    private readonly ISlackMessageAnonymizerService anonymizerService = anonymizerService;

    public IMessagesService Create(AnonymizeConsoleCommand consoleCommand)
    {
        var outputDirectoryPath = CreateOutputDirectoryPath(consoleCommand);
        var senstivieDataPath = CreateSensitiveDataPath(outputDirectoryPath);

        var messagesReadRepository = readRepositoryFactory.Create(consoleCommand.SourceDirectory);
        var messagesWriteRepository = writeRepositoryFactory.Create(outputDirectoryPath, consoleCommand.AggregationMode);
        var sensitiveDataWriteRepository = sensitiveDataWriteRepositoryFactory.Create(senstivieDataPath);

        return new MessagesService(
            messagesReadRepository,
            messagesWriteRepository,
            sensitiveDataWriteRepository,
            anonymousIdFactory,
            anonymizerService);
    }

    private static string CreateSensitiveDataPath(string outputDirectoryPath)
    {
        return Path.Combine(outputDirectoryPath, "sensitive-data.json");
    }

    private static string CreateOutputDirectoryPath(AnonymizeConsoleCommand consoleCommand)
    {
        return Path.Combine(consoleCommand.SourceDirectory, consoleCommand.TargetDirectory);
    }
}
