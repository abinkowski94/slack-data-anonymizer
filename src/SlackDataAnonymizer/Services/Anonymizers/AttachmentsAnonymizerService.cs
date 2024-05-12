using SlackDataAnonymizer.Abstractions.Models;
using SlackDataAnonymizer.Abstractions.Service;
using SlackDataAnonymizer.Models.Slack;

namespace SlackDataAnonymizer.Services.Anonymizers;

public class AttachmentsAnonymizerService(
    IAnonymizerService<string> textAnonymizer) : IAnonymizerService<Attachment>
{
    private readonly IAnonymizerService<string> textAnonymizer = textAnonymizer;

    public Attachment? Anonymize(Attachment? value, ISensitiveData sensitiveData)
    {
        if (value is null)
        {
            return value;
        }

        value.Text = textAnonymizer.Anonymize(value.Text, sensitiveData);

        return value;
    }
}
