using Cocona;
using SlackDataAnonymizer.Commands;

namespace SlackDataAnonymizer.Handlers;

public class AnonymizeCommandHandler
{
    [Command("anonymize")]
    public async ValueTask AnonymizeAsync(AnonymizeCommand command, CancellationToken cancellationToken)
    {

    }
}
