namespace SlackDataAnonymizer.Commands;

public class AnonymizeDataCommand
{
    public required IReadOnlyList<string> TextTags { get; init; }
}
