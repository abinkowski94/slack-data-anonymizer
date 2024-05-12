using System.Text.Json.Serialization;

namespace SlackDataAnonymizer.Models.Slack;

public class Reaction
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("users")]
    public string[]? Users { get; set; }

    [JsonPropertyName("count")]
    public int Count { get; set; }
}
