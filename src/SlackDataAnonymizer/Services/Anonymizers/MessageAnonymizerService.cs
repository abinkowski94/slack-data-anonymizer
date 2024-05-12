using SlackDataAnonymizer.Abstractions.Models;
using SlackDataAnonymizer.Abstractions.Service;
using SlackDataAnonymizer.Models.Slack;

namespace SlackDataAnonymizer.Services.Anonymizers;

public class MessageAnonymizerService(
    IAnonymizerService<string> textAnonymizer,
    IAnonymizerService<Reply> repliesAnonymizer,
    IAnonymizerService<Reaction> reactionAnonymizer) : IAnonymizerService<SlackMessage>
{
    private readonly IAnonymizerService<string> textAnonymizer = textAnonymizer;
    private readonly IAnonymizerService<Reply> repliesAnonymizer = repliesAnonymizer;
    private readonly IAnonymizerService<Reaction> reactionAnonymizer = reactionAnonymizer;

    public SlackMessage? Anonymize(SlackMessage? value, ISensitiveData sensitiveData)
    {
        if (value is null)
        {
            return value;
        }

        AnonymizeUser(value, sensitiveData);
        AnonymizeUserProfile(value);
        AnonymizeReplyUsers(value, sensitiveData);
        AnonymizeParentUserId(value, sensitiveData);

        value.Text = textAnonymizer.Anonymize(value.Text, sensitiveData);
        repliesAnonymizer.Anonymize(value.Replies, sensitiveData);
        reactionAnonymizer.Anonymize(value.Reactions, sensitiveData);

        return value;
    }

    private static void AnonymizeUser(SlackMessage message, ISensitiveData sensitiveData)
    {
        if (message.User is null)
        {
            return;
        }

        message.User = sensitiveData.GetOrAddUser(message.User, message.UserProfile);
    }

    private static void AnonymizeUserProfile(SlackMessage message)
    {
        message.UserProfile = null;
    }

    private static void AnonymizeReplyUsers(SlackMessage message, ISensitiveData sensitiveData)
    {
        if (message.ReplyUsers is null)
        {
            return;
        }

        for (var index = 0; index < message.ReplyUsers.Length; index++)
        {
            message.ReplyUsers[index] = sensitiveData.GetOrAddUser(message.ReplyUsers[index]);
        }
    }

    private static void AnonymizeParentUserId(SlackMessage message, ISensitiveData sensitiveData)
    {
        if (message.ParentUserId is null)
        {
            return;
        }

        message.ParentUserId = sensitiveData.GetOrAddUser(message.ParentUserId);
    }
}
