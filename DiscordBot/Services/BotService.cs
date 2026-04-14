using Discord;
using Discord.WebSocket;
using DiscordBot.Handlers;
using Microsoft.Extensions.Configuration;

namespace DiscordBot.Services;

public class BotService
{
    private readonly DiscordSocketClient _client;
    private readonly CommandHandler _commandHandler;
    private readonly IConfiguration _config;

    public BotService(
        DiscordSocketClient client,
        CommandHandler commandHandler,
        IConfiguration config
        )
    {
        _client = client;
        _commandHandler = commandHandler;
        _config = config;
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _client.MessageReceived += _commandHandler.HandleMessageAsync;

        bool validateToken = true;
        
        var token = _config["Discord:Token"];
        await _client.LoginAsync(TokenType.Bot,
            token, 
            validateToken);
        
        await _client.StartAsync();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _client.StopAsync();
    }
    
}