using SlackDataAnonymizer.Models.Slack;
using System.Text.Json.Serialization;

namespace SlackDataAnonymizer.Models.Maps;

public class UserDetails
{
    [JsonPropertyName("anonymized_id")]
    public required string AnonymizedId { get; init; }

    [JsonPropertyName("user_id")]
    public required string UserId { get; init; }

    [JsonPropertyName("user_profile")]
    public UserProfile? Profile { get; set; }
}
