using SlackDataAnonymizer.Abstractions.Models;
using SlackDataAnonymizer.Abstractions.Service;
using SlackDataAnonymizer.Commands;
using SlackDataAnonymizer.Models.Slack;

namespace SlackDataAnonymizer.Services.Anonymizers;

public class ReactionsAnonymizerService : IAnonymizerService<Reaction>
{
    public Reaction? Anonymize(Reaction? value, AnonymizeDataCommand command, ISensitiveData sensitiveData)
    {
        if (value is null || value.Users is null)
        {
            return value;
        }

        for (int index = 0; index < value.Users.Length; index++)
        {
            value.Users[index] = sensitiveData.GetOrAddUser(value.Users[index]);
        }

        return value;
    }
}
