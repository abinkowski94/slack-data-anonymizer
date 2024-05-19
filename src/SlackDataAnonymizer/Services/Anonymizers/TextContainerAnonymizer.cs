using SlackDataAnonymizer.Abstractions.Models;
using SlackDataAnonymizer.Abstractions.Service;
using SlackDataAnonymizer.Models.Slack;

namespace SlackDataAnonymizer.Services.Anonymizers;

public class TextContainerAnonymizer(IAnonymizerService<string> textAnonymizer) : IAnonymizerService<TextContainer>
{
    private readonly IAnonymizerService<string> textAnonymizer = textAnonymizer;

    public TextContainer? Anonymize(TextContainer? value, ISensitiveData sensitiveData)
    {
        if (value is null)
        {
            return null;
        }

        value.Text = textAnonymizer.Anonymize(value.Text, sensitiveData);

        return value;
    }
}
