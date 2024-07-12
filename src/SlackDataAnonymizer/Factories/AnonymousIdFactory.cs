using SlackDataAnonymizer.Abstractions.Factories;

namespace SlackDataAnonymizer.Factories;

public class AnonymousIdFactory : IAnonymousIdFactory
{
    public string GetId()
    {
        return Guid.NewGuid().ToString("N");
    }
}
