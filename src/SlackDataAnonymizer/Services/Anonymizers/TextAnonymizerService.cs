using SlackDataAnonymizer.Abstractions.Models;
using SlackDataAnonymizer.Abstractions.Service;
using System.Text.RegularExpressions;

namespace SlackDataAnonymizer.Services.Anonymizers;

public partial class TextAnonymizerService : IAnonymizerService<string>
{
    [GeneratedRegex("<@.*>", RegexOptions.Compiled)]
    private static partial Regex UserIdPattern();
    private static readonly Regex userIdPattern = UserIdPattern();

    public string? Anonymize(string? value, ISensitiveData sensitiveData)
    {
        if (value is null)
        {
            return value;
        }

        var matchedValues = userIdPattern.Matches(value);
        var matches = matchedValues.AsEnumerable().ToList();

        var matchReplacements = new Dictionary<string, string>();

        foreach (var item in matches)
        {
            var userId = item.ValueSpan[2..^1].ToString();
            var anonymizedId = sensitiveData.GetOrAddUser(userId);

            matchReplacements[userId] = anonymizedId;
        }

        return matchReplacements.Aggregate(value, (current, replacement) => current.Replace(replacement.Key, replacement.Value));
    }
}
