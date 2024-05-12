using SlackDataAnonymizer.Abstractions.Models;
using SlackDataAnonymizer.Abstractions.Repositories;
using SlackDataAnonymizer.Abstractions.Service;
using SlackDataAnonymizer.Models.Maps;
using SlackDataAnonymizer.Models.Slack;
using System.Runtime.CompilerServices;

namespace SlackDataAnonymizer.Services;

public class MessagesService(
    IMessagesReadRepository readRepository,
    IMessagesWriteRepository writeRepository,
    ISensitiveDataWriteRepository sensitiveDataRepository,
    IAnonymizerService<SlackMessage> anonymizerService) : IMessagesService
{
    private readonly IMessagesReadRepository readRepository = readRepository;
    private readonly IMessagesWriteRepository writeRepository = writeRepository;
    private readonly ISensitiveDataWriteRepository sensitiveDataRepository = sensitiveDataRepository;

    private readonly IAnonymizerService<SlackMessage> anonymizerService = anonymizerService;

    public async ValueTask AnonymizeMessagesAsync(CancellationToken cancellationToken)
    {
        var sensitiveData = new SensitiveData();
        var messages = GetAnonymizedMessagesAsync(sensitiveData, cancellationToken);

        await writeRepository.CreateSlackMessagesAsync(messages, cancellationToken).ConfigureAwait(false);
        await sensitiveDataRepository.CreateSensitiveDataAsync(sensitiveData, cancellationToken).ConfigureAwait(false);
    }

    private async IAsyncEnumerable<SlackMessage> GetAnonymizedMessagesAsync(
        ISensitiveData sensitiveData,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var messages = readRepository.GetSlackMessagesAsync(cancellationToken);

        await foreach (var message in messages)
        {
            anonymizerService.Anonymize(message, sensitiveData);

            yield return message;
        }
    }
}
