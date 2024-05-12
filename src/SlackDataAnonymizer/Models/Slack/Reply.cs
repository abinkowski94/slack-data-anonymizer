using System.Text.Json.Serialization;

namespace SlackDataAnonymizer.Models.Slack;

public class Reply
{
    [JsonPropertyName("user")]
    public string? User { get; set; }

    [JsonPropertyName("ts")]
    public string? TimeStamp { get; set; }
}
