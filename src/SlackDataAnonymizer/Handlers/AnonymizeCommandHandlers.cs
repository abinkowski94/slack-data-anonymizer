using Cocona;
using SlackDataAnonymizer.Abstractions.Factories;
using SlackDataAnonymizer.Abstractions.Service;
using SlackDataAnonymizer.Commands;
using SlackDataAnonymizer.Models.Maps;

namespace SlackDataAnonymizer.Handlers;

public class AnonymizeCommandHandlers(
    IMessagesReadRepositoryFactory readRepositoryFactory,
    ISensitiveDataWriteRepositoryFactory sensitiveDataWriteRepositoryFactory,
    ISlackMessageAnonymizerService anonymizerService)
{
    private readonly IMessagesReadRepositoryFactory readRepositoryFactory = readRepositoryFactory;
    private readonly ISensitiveDataWriteRepositoryFactory sensitiveDataWriteRepositoryFactory = sensitiveDataWriteRepositoryFactory;
    private readonly ISlackMessageAnonymizerService anonymizerService = anonymizerService;

    [Command("anonymize")]
    public async ValueTask AnonymizeAsync(
        AnonymizeCommand command,
        [Ignore] CancellationToken cancellationToken = default)
    {
        var senstivieDataPath = Path.Combine(command.SourceDirectory, command.TargetDirectory, "sensitive-data.json");

        var messagesReadRepository = readRepositoryFactory.Create(command.SourceDirectory);
        var sensitiveDataWriteRepository = sensitiveDataWriteRepositoryFactory.Create(senstivieDataPath);

        var sensitvieData = new SensitiveData();
        var no = 1;

        await foreach(var message in messagesReadRepository.GetSlackMessagesAsync(cancellationToken))
        {
            anonymizerService.Anonymize(message, sensitvieData);

            var textChars = (message.Text.Match(c => c?.Text, s => s) ?? string.Empty)
                .Except("\n\r")
                .Take(61)
                .Concat("...")
                .ToArray();

            var text = new string(textChars);

            await Console.Out.WriteLineAsync($"{no++}. {text}");
        }

        await Console.Out.WriteLineAsync();
        await Console.Out.WriteLineAsync($"Users: {sensitvieData.UserIds.Count}");

        await sensitiveDataWriteRepository.CreateSensitiveDataAsync(sensitvieData, cancellationToken);
    }
}
