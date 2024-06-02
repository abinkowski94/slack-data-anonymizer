using SlackDataAnonymizer.Models.Maps;
using SlackDataAnonymizer.Models.Slack;
using System.Text.Json.Serialization;

namespace SlackDataAnonymizer.Abstractions.Models;

public interface ISensitiveData
{
    [JsonPropertyName("user_ids")]
    IReadOnlyDictionary<string, string> UserIds { get; }

    [JsonPropertyName("user_details")]
    IReadOnlyDictionary<string, UserDetails> UserDetails { get; }

    [JsonPropertyName("text_tags")]
    IReadOnlyDictionary<string, string> TextTags { get; }

    string GetOrAddUser(string userId, UserProfile? userProfile = null);
    void UpdateUserProfileWithId(string userId, UserProfile? userProfile = null);
    void UpdateUserProfileWithAnonymizedId(string anonymizedUserId, UserProfile? userProfile = null);

    string GetOrAddTag(string tag);
}