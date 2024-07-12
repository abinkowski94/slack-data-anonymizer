using Cocona.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace SlackDataAnonymizer.Abstractions;

public interface IStartup
{
    void Configure(ICoconaCommandsBuilder builder);
    void ConfigureServices(IServiceCollection services);
}