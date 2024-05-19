using Cocona;
using SlackDataAnonymizer.Models.Enums;

namespace SlackDataAnonymizer.Commands;

public class AnonymizeCommand : ICommandParameterSet
{
    [Option]
    public required string SourceDirectory { get; init; }

    [HasDefaultValue]
    [Option]
    public string TargetDirectory { get; init; } = ".\\anonymized";

    [HasDefaultValue]
    [Option]
    public AggregationMode AggregationMode { get; init; } = AggregationMode.Daily;
}
