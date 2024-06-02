using Cocona;
using Cocona.Builder;
using Microsoft.Extensions.DependencyInjection;
using OneOf;
using SlackDataAnonymizer.Abstractions.Factories;
using SlackDataAnonymizer.Abstractions.Service;
using SlackDataAnonymizer.Factories;
using SlackDataAnonymizer.Handlers;
using SlackDataAnonymizer.Models.Slack;
using SlackDataAnonymizer.Serialization;
using SlackDataAnonymizer.Services.Anonymizers;

var builder = CoconaApp.CreateBuilder();

AddAnonymizers(builder);
AddFactories(builder);

var app = builder.Build();

app.AddCommands<AnonymizeCommandHandlers>();

await app.RunAsync();

static CoconaAppBuilder AddAnonymizers(CoconaAppBuilder builder)
{
    builder.Services.AddSingleton<IAnonymizerService<string>, TextAnonymizerService>();
    builder.Services.AddSingleton<IAnonymizerService<TextContainer>, TextContainerAnonymizer>();
    builder.Services.AddSingleton<IAnonymizerService<OneOf<TextContainer?, string?>>, OneOfTextContainerAnonymizer>();
    builder.Services.AddSingleton<IAnonymizerService<Element>, ElementsAnonymizerService>();
    builder.Services.AddSingleton<IAnonymizerService<Reply>, RepliesAnonymizerService>();
    builder.Services.AddSingleton<IAnonymizerService<Reaction>, ReactionsAnonymizerService>();
    builder.Services.AddSingleton<IAnonymizerService<Block>, BlocksAnonymizerService>();
    builder.Services.AddSingleton<IAnonymizerService<Attachment>, AttachmentsAnonymizerService>();

    builder.Services.AddSingleton<MessageAnonymizerService>();
    builder.Services.AddSingleton<IAnonymizerService<SlackMessage>>(sp => sp.GetRequiredService<MessageAnonymizerService>());
    builder.Services.AddSingleton<ISlackMessageAnonymizerService>(sp => sp.GetRequiredService<MessageAnonymizerService>());

    return builder;
}

static CoconaAppBuilder AddFactories(CoconaAppBuilder builder)
{
    builder.Services.AddSingleton(SerializationConsts.Options);
    builder.Services.AddSingleton<IMessagesReadRepositoryFactory, MessagesReadRepositoryFactory>();
    builder.Services.AddSingleton<IMessagesWriteRepositoryFactory, MessagesWriteRepositoryFactory>();
    builder.Services.AddSingleton<ISensitiveDataWriteRepositoryFactory, SensitiveDataWriteRepositoryFactory>();
    builder.Services.AddSingleton<IMessagesServiceFactory, MessagesServiceFactory>();

    return builder;
}