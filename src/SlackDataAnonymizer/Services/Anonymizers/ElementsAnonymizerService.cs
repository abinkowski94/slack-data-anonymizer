﻿using OneOf;
using SlackDataAnonymizer.Abstractions.Models;
using SlackDataAnonymizer.Abstractions.Service;
using SlackDataAnonymizer.Models.Slack;

namespace SlackDataAnonymizer.Services.Anonymizers;

public class ElementsAnonymizerService(
    IAnonymizerService<OneOf<TextContainer?, string?>> textOneOfAnonymizer) : IAnonymizerService<Element>
{
    private readonly IAnonymizerService<OneOf<TextContainer?, string?>> textOneOfAnonymizer = textOneOfAnonymizer;

    public Element? Anonymize(Element? value, ISensitiveData sensitiveData)
    {
        if (value is null)
        {
            return value;
        }

        AnonymizeUserId(value, sensitiveData);
        AnonymizeText(value, sensitiveData);        
        AnonymizeSubElements(value, sensitiveData);

        return value;
    }

    private static void AnonymizeUserId(Element value, ISensitiveData sensitiveData)
    {
        if (value.UserId is null)
        {
            return;
        }

        value.UserId = sensitiveData.GetOrAddUser(value.UserId);
    }

    private void AnonymizeText(Element value, ISensitiveData sensitiveData)
    {
        value.Text = textOneOfAnonymizer.Anonymize(value.Text, sensitiveData);
    }

    private void AnonymizeSubElements(Element value, ISensitiveData sensitiveData)
    {
        if (value.Elements is null)
        {
            return;
        }

        foreach (var subElement in value.Elements)
        {
            Anonymize(subElement, sensitiveData);
        }
    }
}
