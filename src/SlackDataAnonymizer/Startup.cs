using Cocona;
using Cocona.Builder;
using Microsoft.Extensions.DependencyInjection;
using OneOf;
using SlackDataAnonymizer.Abstractions;
using SlackDataAnonymizer.Abstractions.Factories;
using SlackDataAnonymizer.Abstractions.Service;
using SlackDataAnonymizer.Factories;
using SlackDataAnonymizer.Handlers;
using SlackDataAnonymizer.Models.Slack;
using SlackDataAnonymizer.Serialization;
using SlackDataAnonymizer.Services.Anonymizers;

namespace SlackDataAnonymizer;

public class Startup : IStartup
{
    public virtual void ConfigureServices(IServiceCollection services)
    {
        AddAnonymizers(services);
        AddFactories(services);
    }

    public virtual void Configure(ICoconaCommandsBuilder builder)
    {
        AddCommands(builder);
    }

    protected virtual IServiceCollection AddAnonymizers(IServiceCollection services)
    {
        services.AddSingleton<IAnonymizerService<string>, TextAnonymizerService>();
        services.AddSingleton<IAnonymizerService<TextContainer>, TextContainerAnonymizer>();
        services.AddSingleton<IAnonymizerService<OneOf<TextContainer?, string?>>, OneOfTextContainerAnonymizer>();
        services.AddSingleton<IAnonymizerService<Element>, ElementsAnonymizerService>();
        services.AddSingleton<IAnonymizerService<Reply>, RepliesAnonymizerService>();
        services.AddSingleton<IAnonymizerService<Reaction>, ReactionsAnonymizerService>();
        services.AddSingleton<IAnonymizerService<Block>, BlocksAnonymizerService>();
        services.AddSingleton<IAnonymizerService<Attachment>, AttachmentsAnonymizerService>();

        services.AddSingleton<MessageAnonymizerService>();
        services.AddSingleton<IAnonymizerService<SlackMessage>>(sp => sp.GetRequiredService<MessageAnonymizerService>());
        services.AddSingleton<ISlackMessageAnonymizerService>(sp => sp.GetRequiredService<MessageAnonymizerService>());

        return services;
    }

    protected virtual IServiceCollection AddFactories(IServiceCollection services)
    {
        services.AddSingleton(SerializationConsts.Options);
        services.AddSingleton<IMessagesReadRepositoryFactory, MessagesReadRepositoryFactory>();
        services.AddSingleton<IMessagesWriteRepositoryFactory, MessagesWriteRepositoryFactory>();
        services.AddSingleton<ISensitiveDataWriteRepositoryFactory, SensitiveDataWriteRepositoryFactory>();
        services.AddSingleton<IMessagesServiceFactory, MessagesServiceFactory>();
        services.AddSingleton<IAnonymousIdFactory, AnonymousIdFactory>();

        return services;
    }

    protected virtual ICoconaCommandsBuilder AddCommands(ICoconaCommandsBuilder builder)
    {
        builder.AddCommands<AnonymizeCommandHandlers>();

        return builder;
    }
}
