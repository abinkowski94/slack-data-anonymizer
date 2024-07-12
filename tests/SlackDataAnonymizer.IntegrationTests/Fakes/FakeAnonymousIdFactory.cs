using SlackDataAnonymizer.Abstractions.Factories;

namespace SlackDataAnonymizer.IntegrationTests.Fakes;

public class FakeAnonymousIdFactory : IAnonymousIdFactory
{
    private int currentId;

    public string GetId()
    {
        var nextId = Interlocked.Increment(ref currentId);

        return nextId.ToString().PadLeft(32, '0');
    }
}
