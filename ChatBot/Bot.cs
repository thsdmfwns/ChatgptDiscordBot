using System.Reflection;
using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;

namespace ChatBot;

public class Bot
{
    private DiscordSocketClient _client;
    private CommandService _commands;
    private InteractionService _interactionService;
    private readonly IServiceProvider _services;

    public Bot(DiscordSocketClient client, CommandService commands, InteractionService interactionService, IServiceProvider services)
    {
        _client = client;
        _commands = commands;
        _interactionService = interactionService;
        _services = services;
    }

    public async Task Run(string token)
    {
        _client.Log += OnClientLogReceived;
        _commands.Log += OnClientLogReceived;

        await _client.LoginAsync(TokenType.Bot, token);
        await _client.StartAsync();
        _client.MessageReceived += OnClientMessage;
        _client.Ready += OnClientReady;

        await _commands.AddModulesAsync(assembly: Assembly.GetEntryAssembly(), services: null);
        await Task.Delay(-1);
    }

    private async Task OnClientReady()
    {
        await _interactionService.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        await _interactionService.RegisterCommandsGloballyAsync(true);
        _client.InteractionCreated += OnInteractionCreated;
    }

    private async Task OnInteractionCreated(SocketInteraction arg)
    {
        try
        {
            var ctx = new SocketInteractionContext(_client, arg);
            await _interactionService.ExecuteCommandAsync(ctx, _services);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            if (arg.Type == InteractionType.ApplicationCommand)
                await arg.GetOriginalResponseAsync().ContinueWith(async (msg) => await msg.Result.DeleteAsync());
        }
    }

    private async Task OnClientMessage(SocketMessage arg)
    {
        SocketUserMessage? message = arg as SocketUserMessage;
        if (message == null) return;

        int pos = 0;

        if (!(message.HasCharPrefix('!', ref pos) ||
              message.HasMentionPrefix(_client.CurrentUser, ref pos)) ||
            message.Author.IsBot)
            return;

        var context = new SocketCommandContext(_client, message);

        await _commands.ExecuteAsync(context: context, argPos: pos, services: null);
    }

    private Task OnClientLogReceived(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());  //로그 출력
        return Task.CompletedTask;
    }
}