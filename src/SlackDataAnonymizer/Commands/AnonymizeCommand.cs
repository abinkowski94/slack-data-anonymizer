using SlackDataAnonymizer.Models.Enums;

namespace SlackDataAnonymizer.Commands;

public class AnonymizeCommand
{
    public required string SourceDirectory { get; init; }

    public string? TargetDirectory { get; init; }

    public AggregationMode AggregationMode { get; init; }
}
