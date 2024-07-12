using Cocona;
using SlackDataAnonymizer;
using SlackDataAnonymizer.Extensions;

var builder = CoconaApp.CreateBuilder(args)
    .UseStartup<Startup>();

using var app = builder.Build()
    .UseStartup<Startup>();

await app.RunAsync();
