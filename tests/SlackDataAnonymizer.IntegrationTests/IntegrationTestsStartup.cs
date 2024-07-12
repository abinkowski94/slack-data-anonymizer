using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SlackDataAnonymizer.Abstractions.Factories;
using SlackDataAnonymizer.Factories;
using SlackDataAnonymizer.IntegrationTests.Fakes;

namespace SlackDataAnonymizer.IntegrationTests;

public class IntegrationTestsStartup : Startup
{
    public override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);

        ReplaceAnonymousIdFactory(services);
    }

    private static void ReplaceAnonymousIdFactory(IServiceCollection services)
    {
        services.RemoveAll<IAnonymousIdFactory>();
        services.RemoveAll<AnonymousIdFactory>();

        services.AddSingleton<IAnonymousIdFactory, FakeAnonymousIdFactory>();
    }
}
