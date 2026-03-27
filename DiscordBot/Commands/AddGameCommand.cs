using Discord.WebSocket;
using DiscordBot.Models;
using DiscordBot.Services.Interfaces;

namespace DiscordBot.Commands;

public class AddGameCommand : ICommand
{
    private readonly IGameService _gameService;
    
    public string Name => "addgame";
    
    public AddGameCommand(
        IGameService gameService
        )
    {
        _gameService = gameService;
    }

    public async Task ExecuteAsync(SocketMessage socketMessage, string[] arg)
    {
        var game = await _gameService.AddGameAsync(arg);

        await socketMessage.Channel.SendMessageAsync($"Game added: {game.Name}");
    }
    
}