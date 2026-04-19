using System.Reflection;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using DiscordBot.Handlers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace DiscordBot.Services;

public class BotService
{
    private readonly DiscordSocketClient _client;
    private readonly CommandHandler _commandHandler;
    private readonly IConfiguration _config;
    private readonly InteractionService _interactionService;
    private readonly IServiceProvider _services;
    private readonly IHostEnvironment _environment;

    public BotService(
        DiscordSocketClient client,
        CommandHandler commandHandler,
        IConfiguration config,
        InteractionService interactionService,
        IServiceProvider services,
        IHostEnvironment environment
        )
    {
        _client = client;
        _commandHandler = commandHandler;
        _config = config;
        _interactionService = interactionService;
        _services = services;
        _environment = environment;
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
            if (_environment.IsDevelopment())
            {
                await _interactionService.RegisterCommandsToGuildAsync(guildId);
            }
            else
            {
                await _interactionService.RegisterCommandsGloballyAsync();
            }
        };
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _client.StopAsync();
    }
    
}