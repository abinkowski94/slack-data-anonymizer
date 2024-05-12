using System.Text.Json.Serialization;

namespace SlackDataAnonymizer.Models.Slack;

public class Field
{
    [JsonPropertyName("value")]
    public string? Value { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }
}
