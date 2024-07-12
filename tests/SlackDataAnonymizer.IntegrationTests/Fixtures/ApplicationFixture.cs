using Cocona;
using SlackDataAnonymizer.Extensions;

namespace SlackDataAnonymizer.IntegrationTests.Fixtures;

public static class ApplicationFixture
{
    public static CoconaApp Create(params string[] args)
    {
        var builder = CoconaApp.CreateBuilder(args)
            .UseStartup<IntegrationTestsStartup>();

        return builder.Build()
            .UseStartup<IntegrationTestsStartup>();
    }
}
