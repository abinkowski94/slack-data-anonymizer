using SlackDataAnonymizer.Abstractions.Factories;
using SlackDataAnonymizer.Abstractions.Models;
using SlackDataAnonymizer.Abstractions.Repositories.Read;
using SlackDataAnonymizer.Abstractions.Repositories.Write;
using SlackDataAnonymizer.Abstractions.Service;
using SlackDataAnonymizer.Commands;
using SlackDataAnonymizer.Models.Maps;
using SlackDataAnonymizer.Models.Slack;
using System.Runtime.CompilerServices;

namespace SlackDataAnonymizer.Services;

public class MessagesService(
    IMessagesReadRepository readRepository,
    IMessagesWriteRepository writeRepository,
    ISensitiveDataWriteRepository sensitiveDataRepository,
    IAnonymousIdFactory anonymousIdFactory,
    ISlackMessageAnonymizerService anonymizerService)
    : IMessagesService
{
    private readonly IMessagesReadRepository readRepository = readRepository;
    private readonly IMessagesWriteRepository writeRepository = writeRepository;
    private readonly ISensitiveDataWriteRepository sensitiveDataRepository = sensitiveDataRepository;

    private readonly IAnonymousIdFactory anonymousIdFactory = anonymousIdFactory;
    private readonly ISlackMessageAnonymizerService anonymizerService = anonymizerService;

    public async ValueTask AnonymizeMessagesAsync(AnonymizeDataCommand command, CancellationToken cancellationToken)
    {
        var sensitiveData = new SensitiveData(anonymousIdFactory);
        var messages = GetAnonymizedMessagesAsync(command, sensitiveData, cancellationToken);

        await writeRepository.CreateSlackMessagesAsync(messages, cancellationToken).ConfigureAwait(false);
        await sensitiveDataRepository.CreateSensitiveDataAsync(sensitiveData, cancellationToken).ConfigureAwait(false);
    }

    private async IAsyncEnumerable<SlackMessage> GetAnonymizedMessagesAsync(
        AnonymizeDataCommand command,
        ISensitiveData sensitiveData,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var messages = readRepository.GetSlackMessagesAsync(cancellationToken);

        await foreach (var message in messages)
        {
            anonymizerService.Anonymize(message, command, sensitiveData);

            yield return message;
        }
    }
}
