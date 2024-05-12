using System.Text.Json.Serialization;

namespace SlackDataAnonymizer.Models.Slack;

public class Attachment
{
    [JsonPropertyName("from_url")]
    public string? FromUrl { get; set; }

    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("original_url")]
    public string? OriginalUrl { get; set; }

    [JsonPropertyName("fallback")]
    public string? Fallback { get; set; }

    [JsonPropertyName("text")]
    public string? Text { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("title_link")]
    public string? TitleLink { get; set; }

    [JsonPropertyName("fields")]
    public Field[]? Fields { get; set; }
}
