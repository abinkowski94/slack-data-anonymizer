using SlackDataAnonymizer.Abstractions.Models;
using SlackDataAnonymizer.Abstractions.Service;
using SlackDataAnonymizer.Models.Slack;

namespace SlackDataAnonymizer.Services.Anonymizers;

public class BlocksAnonymizerService(
    IAnonymizerService<Element> elementsAnonymizer) : IAnonymizerService<Block>
{
    private readonly IAnonymizerService<Element> elementsAnonymizer = elementsAnonymizer;

    public Block? Anonymize(Block? value, ISensitiveData sensitiveData)
    {
        if (value is null)
        {
            return value;
        }

        if (value.Elements is null)
        {
            return value;
        }

        foreach (var element in value.Elements)
        {
            elementsAnonymizer.Anonymize(element, sensitiveData);
        }

        return value;
    }
}
