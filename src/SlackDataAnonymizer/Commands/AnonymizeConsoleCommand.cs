using Cocona;
using SlackDataAnonymizer.Models.Enums;

namespace SlackDataAnonymizer.Commands;

public class AnonymizeConsoleCommand : ICommandParameterSet
{
    [Option]
    public required string SourceDirectory { get; init; }

    [HasDefaultValue]
    [Option]
    public string TargetDirectory { get; init; } = ".\\anonymized";

    [HasDefaultValue]
    [Option]
    public AggregationMode AggregationMode { get; init; } = AggregationMode.Daily;

    [HasDefaultValue]
    [Option]
    public string[] TextTags { get; init; } = [];
}
