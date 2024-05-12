using System.Text.Json.Serialization;

namespace SlackDataAnonymizer.Models.Slack;

public class Element
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("elements")]
    public Element[]? Elements { get; set; }

    [JsonPropertyName("text")]
    public string? Text { get; set; }

    [JsonPropertyName("url")]
    public string? Url { get; set; }

    [JsonPropertyName("user_id")]
    public string? UserId { get; set; }
}
