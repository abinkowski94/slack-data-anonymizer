using SlackDataAnonymizer.Abstractions.Models;
using SlackDataAnonymizer.Models.Slack;
using System.Text.Json.Serialization;

namespace SlackDataAnonymizer.Models.Maps;

public class SensitiveData : ISensitiveData
{
    private readonly Dictionary<string, string> userIds;
    private readonly Dictionary<string, UserDetails> userDetails;

    [JsonPropertyName("user_ids")]
    public IReadOnlyDictionary<string, string> UserIds { get; }

    [JsonPropertyName("user_details")]
    public IReadOnlyDictionary<string, UserDetails> UserDetails { get; }

    public SensitiveData() :
        this(
            new Dictionary<string, string>(),
            new Dictionary<string, UserDetails>())
    {
    }

    public SensitiveData(
        IReadOnlyDictionary<string, string> userIds,
        IReadOnlyDictionary<string, UserDetails> userDetails)
    {
        this.userIds = userIds.ToDictionary(kv => kv.Key, kv => kv.Value);
        this.userDetails = userDetails.ToDictionary(kv => kv.Key, kv => kv.Value);

        UserIds = this.userIds.AsReadOnly();
        UserDetails = this.userDetails.AsReadOnly();
    }

    public string GetOrAddUser(string userId, UserProfile? userProfile = null)
    {
        if (!userIds.TryGetValue(userId, out var result))
        {
            result = CreateUser(userId, userProfile).AnonymizedId;
        }

        UpdateUserProfileWithId(userId, userProfile);

        return result;
    }

    public void UpdateUserProfileWithId(string userId, UserProfile? userProfile = null)
    {
        if (userProfile is null)
        {
            return;
        }

        var anonymizedId = UserIds[userId];

        UpdateUserProfileWithAnonymizedId(anonymizedId, userProfile);
    }

    public void UpdateUserProfileWithAnonymizedId(string anonymizedUserId, UserProfile? userProfile = null)
    {
        if (userProfile is null)
        {
            return;
        }

        var userDetails = UserDetails[anonymizedUserId];

        if (userDetails.Profile is null)
        {
            userDetails.Profile = userProfile;
        }
        else
        {
            userDetails.Profile.MergeWith(userProfile);
        }
    }

    private UserDetails CreateUser(string userId, UserProfile? userProfile)
    {
        var anonymizedId = Guid.NewGuid().ToString("N");
        var userDetails = new UserDetails { AnonymizedId = anonymizedId, UserId = userId, Profile = userProfile };

        userIds.Add(userId, anonymizedId);
        this.userDetails.Add(anonymizedId, userDetails);

        return userDetails;
    }
}
