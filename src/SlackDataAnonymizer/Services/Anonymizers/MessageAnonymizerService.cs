using SlackDataAnonymizer.Abstractions.Models;
using SlackDataAnonymizer.Abstractions.Service;
using SlackDataAnonymizer.Models.Slack;

namespace SlackDataAnonymizer.Services.Anonymizers;

public class MessageAnonymizerService(
    IAnonymizerService<string> textAnonymizer,
    IAnonymizerService<Reply> repliesAnonymizer,
    IAnonymizerService<Reaction> reactionAnonymizer,
    IAnonymizerService<Block> blocksAnonymizer,
    IAnonymizerService<Attachment> attachmentAnonymizer) : IAnonymizerService<SlackMessage>
{
    private readonly IAnonymizerService<string> textAnonymizer = textAnonymizer;
    private readonly IAnonymizerService<Reply> repliesAnonymizer = repliesAnonymizer;
    private readonly IAnonymizerService<Reaction> reactionAnonymizer = reactionAnonymizer;
    private readonly IAnonymizerService<Block> blocksAnonymizer = blocksAnonymizer;
    private readonly IAnonymizerService<Attachment> attachmentAnonymizer = attachmentAnonymizer;

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
        AnonymizeText(value, sensitiveData);

        repliesAnonymizer.Anonymize(value.Replies, sensitiveData);
        reactionAnonymizer.Anonymize(value.Reactions, sensitiveData);
        blocksAnonymizer.Anonymize(value.Blocks, sensitiveData);
        attachmentAnonymizer.Anonymize(value.Attachments, sensitiveData);

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

    private void AnonymizeText(SlackMessage value, ISensitiveData sensitiveData)
    {
        value.Text = textAnonymizer.Anonymize(value.Text, sensitiveData);
    }
}
