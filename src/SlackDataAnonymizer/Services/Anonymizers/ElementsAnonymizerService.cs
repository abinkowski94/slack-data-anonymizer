using SlackDataAnonymizer.Abstractions.Models;
using SlackDataAnonymizer.Abstractions.Service;
using SlackDataAnonymizer.Models.Slack;

namespace SlackDataAnonymizer.Services.Anonymizers;

public class ElementsAnonymizerService(
    IAnonymizerService<string> textAnonymizer) : IAnonymizerService<Element>
{
    private readonly IAnonymizerService<string> textAnonymizer = textAnonymizer;

    public Element? Anonymize(Element? value, ISensitiveData sensitiveData)
    {
        if (value is null)
        {
            return value;
        }

        value.Text = textAnonymizer.Anonymize(value.Text, sensitiveData);

        if (value.Elements is null)
        {
            return value;
        }

        foreach (var subElement in value.Elements)
        {
            Anonymize(subElement, sensitiveData);
        }

        return value;
    }
}
