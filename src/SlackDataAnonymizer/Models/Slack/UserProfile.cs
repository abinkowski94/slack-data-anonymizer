using System.Text.Json.Serialization;

namespace SlackDataAnonymizer.Models.Slack;

public class UserProfile
{
    [JsonPropertyName("first_name")]
    public string? FirstName { get; set; }

    [JsonPropertyName("real_name")]
    public string? RealName { get; set; }

    [JsonPropertyName("display_name")]
    public string? DisplayName { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    public void MergeWith(UserProfile userProfile)
    {
        FirstName = userProfile.FirstName ?? FirstName;
        RealName = userProfile.RealName ?? RealName;
        DisplayName = userProfile.DisplayName ?? DisplayName;
        Name = userProfile.Name ?? Name;
    }
}
