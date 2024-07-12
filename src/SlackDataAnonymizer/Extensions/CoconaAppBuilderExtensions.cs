using Cocona;
using Cocona.Builder;
using SlackDataAnonymizer.Abstractions;

namespace SlackDataAnonymizer.Extensions;

public static class CoconaAppBuilderExtensions
{
    public static CoconaAppBuilder UseStartup<T>(this CoconaAppBuilder builder)
        where T : IStartup, new()
    {
        var startup = new T();
        startup.ConfigureServices(builder.Services);

        return builder;
    }

    public static CoconaApp UseStartup<T>(this CoconaApp app)
        where T : IStartup, new()
    {
        var startup = new T();
        startup.Configure(app);

        return app;
    }
}
