using SlackDataAnonymizer.Abstractions.Models;
using SlackDataAnonymizer.Abstractions.Service;
using SlackDataAnonymizer.Commands;
using SlackDataAnonymizer.Models.Slack;

namespace SlackDataAnonymizer.Services.Anonymizers;

public class RepliesAnonymizerService : IAnonymizerService<Reply>
{
    public Reply? Anonymize(Reply? value, AnonymizeDataCommand command, ISensitiveData sensitiveData)
    {
        if (value is null || value.User is null)
        {
            return value;
        }

        value.User = sensitiveData.GetOrAddUser(value.User);

        return value;
    }
}