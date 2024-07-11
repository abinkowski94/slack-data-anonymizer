using OneOf;
using SlackDataAnonymizer.Abstractions.Models;
using SlackDataAnonymizer.Abstractions.Service;
using SlackDataAnonymizer.Commands;
using SlackDataAnonymizer.Models.Slack;

namespace SlackDataAnonymizer.Services.Anonymizers;

public class OneOfTextContainerAnonymizer(
    IAnonymizerService<TextContainer> textContainerAnonymizer,
    IAnonymizerService<string> textAnonymizer)
    : IAnonymizerService<OneOf<TextContainer?, string?>>
{
    private readonly IAnonymizerService<TextContainer> textContainerAnonymizer = textContainerAnonymizer;
    private readonly IAnonymizerService<string> textAnonymizer = textAnonymizer;

    public OneOf<TextContainer?, string?> Anonymize(OneOf<TextContainer?, string?> value, AnonymizeDataCommand command, ISensitiveData sensitiveData)
    {
        return value.Match<OneOf<TextContainer?, string?>>(c =>
        {
            return textContainerAnonymizer.Anonymize(c, command, sensitiveData);
        },
        s =>
        {
            return textAnonymizer.Anonymize(s, command, sensitiveData);
        });
    }
}
