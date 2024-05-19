using OneOf;
using System.Text.Json.Serialization;

namespace SlackDataAnonymizer.Models.Slack;

public class SlackMessage
{
    [JsonPropertyName("user")]
    public string? User { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("ts")]
    public string? TimeStamp { get; set; }

    [JsonPropertyName("text")]
    public OneOf<TextContainer?, string?> Text { get; set; }

    [JsonPropertyName("user_profile")]
    public UserProfile? UserProfile { get; set; }

    [JsonPropertyName("thread_ts")]
    public string? ThreadTimeStamp { get; set; }

    [JsonPropertyName("reply_count")]
    public int ReplyCount { get; set; }

    [JsonPropertyName("reply_users_count")]
    public int ReplyUsersCount { get; set; }

    [JsonPropertyName("latest_reply")]
    public string? LatestReply { get; set; }

    [JsonPropertyName("reply_users")]
    public string[]? ReplyUsers { get; set; }

    [JsonPropertyName("replies")]
    public Reply[]? Replies { get; set; }

    [JsonPropertyName("blocks")]
    public Block[]? Blocks { get; set; }

    [JsonPropertyName("reactions")]
    public Reaction[]? Reactions { get; set; }

    [JsonPropertyName("parent_user_id")]
    public string? ParentUserId { get; set; }

    [JsonPropertyName("attachments")]
    public Attachment[]? Attachments { get; set; }
}
