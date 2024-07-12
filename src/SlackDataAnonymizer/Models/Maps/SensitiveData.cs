using SlackDataAnonymizer.Abstractions.Factories;
using SlackDataAnonymizer.Abstractions.Models;
using SlackDataAnonymizer.Factories;
using SlackDataAnonymizer.Models.Slack;
using System.Text.Json.Serialization;

namespace SlackDataAnonymizer.Models.Maps;

public class SensitiveData : ISensitiveData
{
    private readonly IAnonymousIdFactory anonymousIdFactory;

    private readonly Dictionary<string, string> userIds;
    private readonly Dictionary<string, UserDetails> userDetails;
    private readonly Dictionary<string, string> textTags;

    [JsonPropertyName("user_ids")]
    public IReadOnlyDictionary<string, string> UserIds { get; }

    [JsonPropertyName("user_details")]
    public IReadOnlyDictionary<string, UserDetails> UserDetails { get; }

    [JsonPropertyName("text_tags")]
    public IReadOnlyDictionary<string, string> TextTags { get; }

    public SensitiveData(
        IAnonymousIdFactory? anonymousIdFactory = null) 
        : this(
              new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase),
              new Dictionary<string, UserDetails>(),
              new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase),
              anonymousIdFactory)
    {
    }

    public SensitiveData(
        IReadOnlyDictionary<string, string> userIds,
        IReadOnlyDictionary<string, UserDetails> userDetails,
        IReadOnlyDictionary<string, string> textTags,
        IAnonymousIdFactory? anonymousIdFactory = null)
    {
        this.userIds = userIds.ToDictionary(kv => kv.Key, kv => kv.Value);
        this.userDetails = userDetails.ToDictionary(kv => kv.Key, kv => kv.Value);
        this.textTags = textTags.ToDictionary(kv => kv.Key, kv => kv.Value);

        UserIds = this.userIds.AsReadOnly();
        UserDetails = this.userDetails.AsReadOnly();
        TextTags = this.textTags.AsReadOnly();

        this.anonymousIdFactory = anonymousIdFactory ?? new AnonymousIdFactory();
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

    public string GetOrAddTag(string tag)
    {
        if (!textTags.TryGetValue(tag, out var result))
        {
            result = anonymousIdFactory.GetId();
            textTags.Add(tag, result);
        }

        return result;
    }

    private UserDetails CreateUser(string userId, UserProfile? userProfile)
    {
        var anonymizedId = anonymousIdFactory.GetId();
        var userDetails = new UserDetails { AnonymizedId = anonymizedId, UserId = userId, Profile = userProfile };

        userIds.Add(userId, anonymizedId);
        this.userDetails.Add(anonymizedId, userDetails);

        return userDetails;
    }
}
