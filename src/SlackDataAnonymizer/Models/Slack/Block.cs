using System.Text.Json.Serialization;

namespace SlackDataAnonymizer.Models.Slack;

public class Block
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("block_id")]
    public string? BlockId { get; set; }

    [JsonPropertyName("elements")]
    public Element[]? Elements { get; set; }
}
