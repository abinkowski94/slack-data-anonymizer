using System.Text.Json.Serialization;

namespace SlackDataAnonymizer.Models.Slack;

public class TextContainer
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("text")]
    public string? Text { get; set; }
}
