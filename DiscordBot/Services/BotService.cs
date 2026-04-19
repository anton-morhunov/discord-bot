using System.Reflection;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using DiscordBot.Handlers;
using Microsoft.Extensions.Configuration;

namespace DiscordBot.Services;

public class BotService
{
    private readonly DiscordSocketClient _client;
    private readonly CommandHandler _commandHandler;
    private readonly IConfiguration _config;
    private readonly InteractionService _interactionService;
    private readonly IServiceProvider _services;

    public BotService(
        DiscordSocketClient client,
        CommandHandler commandHandler,
        IConfiguration config,
        InteractionService interactionService,
        IServiceProvider services
        )
    {
        _client = client;
        _commandHandler = commandHandler;
        _config = config;
        _interactionService = interactionService;
        _services = services;
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _client.MessageReceived += _commandHandler.HandleMessageAsync;

        bool validateToken = true;
        
        var token = _config["Discord:Token"];

        _client.InteractionCreated += async interaction =>
        {
            var ctx = new SocketInteractionContext(_client, interaction);
            await _interactionService.ExecuteCommandAsync(ctx, _services);
        };
        
        await _client.LoginAsync(TokenType.Bot,
            token, 
            validateToken);
        
        await _client.StartAsync();

        _client.Ready += async () =>
        {
            await _interactionService.AddModulesAsync(Assembly.GetEntryAssembly(), _services);

            var guildId = _config.GetValue<ulong>("DiscordId:GuildId");
            await _interactionService.RegisterCommandsToGuildAsync(guildId);
        };
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _client.StopAsync();
    }
    
}