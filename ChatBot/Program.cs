using ChatBot;
using ChatBot.Services;
using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var token = Environment.GetEnvironmentVariable("BOT_TOKEN");
if (token is null)
{
    Console.Error.WriteLine("Discord Bot Token is null");
    Console.Error.WriteLine("Please Set Environment Value \"BOT_TOKEN\" ");
    return;
}

var aiToken = Environment.GetEnvironmentVariable("OPENAI_TOKEN");
if (aiToken is null)
{
    Console.Error.WriteLine("OpenAi Token is null");
    Console.Error.WriteLine("Please Set Environment Value \"OPENAI_TOKEN\" ");
    return;
}

using IHost host = Host.CreateDefaultBuilder()
    .ConfigureServices((_, services) => 
        services
        .AddSingleton<DiscordSocketClient>(x => new DiscordSocketClient(new DiscordSocketConfig()
        {
            LogLevel = LogSeverity.Verbose,
            GatewayIntents = GatewayIntents.All,
        }))
        .AddSingleton<CommandService>(x => new CommandService(new CommandServiceConfig()
        {
            LogLevel = LogSeverity.Verbose
        }))
        .AddSingleton(x => new InteractionService(x.GetRequiredService<DiscordSocketClient>()))
        .AddSingleton<Bot>()
        .AddSingleton<IOpenAi, OpenAi>(x => new OpenAi(aiToken))
    )
.Build();

await host.Services.GetRequiredService<Bot>().Run(token);


    