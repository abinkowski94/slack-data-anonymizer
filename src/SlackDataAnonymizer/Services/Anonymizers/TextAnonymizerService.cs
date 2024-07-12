using SlackDataAnonymizer.Abstractions.Models;
using SlackDataAnonymizer.Abstractions.Service;
using SlackDataAnonymizer.Commands;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace SlackDataAnonymizer.Services.Anonymizers;

public partial class TextAnonymizerService : IAnonymizerService<string>
{
    public string? Anonymize(string? value, AnonymizeDataCommand command, ISensitiveData sensitiveData)
    {
        if (value is null)
        {
            return value;
        }

        var result = AnonymizeUserIds(value, sensitiveData);
        result = AnonymizeTextTags(result, command.TextTags, sensitiveData);

        return result;
    }

    private static string AnonymizeUserIds(string value, ISensitiveData sensitiveData)
    {
        var matchedValues = Regexes.UserIdPattern.Matches(value);
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

    private static string AnonymizeTextTags(string value, IReadOnlyList<string> textTags, ISensitiveData sensitiveData)
    {
        foreach (var textTag in textTags)
        {
            var anonymizedTag = sensitiveData.GetOrAddTag(textTag);

            value = value.Replace(textTag, anonymizedTag, StringComparison.InvariantCultureIgnoreCase);
        }

        return value;
    }

    [ExcludeFromCodeCoverage]
    private static partial class Regexes
    {
        [GeneratedRegex("<@.*?>", RegexOptions.Compiled)]
        private static partial Regex GetUserIdPattern();

        public static Regex UserIdPattern { get; } = GetUserIdPattern();
    }
}
