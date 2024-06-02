using OneOf;
using SlackDataAnonymizer.Abstractions.Models;
using SlackDataAnonymizer.Abstractions.Service;
using SlackDataAnonymizer.Commands;
using SlackDataAnonymizer.Models.Slack;

namespace SlackDataAnonymizer.Services.Anonymizers;

public class AttachmentsAnonymizerService(
    IAnonymizerService<OneOf<TextContainer?, string?>> textOneOfAnonymizer) : IAnonymizerService<Attachment>
{
    private readonly IAnonymizerService<OneOf<TextContainer?, string?>> textOneOfAnonymizer = textOneOfAnonymizer;

    public Attachment? Anonymize(Attachment? value, AnonymizeDataCommand command,  ISensitiveData sensitiveData)
    {
        if (value is null)
        {
            return value;
        }

        value.Text = textOneOfAnonymizer.Anonymize(value.Text, command, sensitiveData);

        return value;
    }
}
