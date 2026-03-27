using Discord.WebSocket;
using DiscordBot.Services.Interfaces;

namespace DiscordBot.Commands;

public class RandomCommand : ICommand
{
    private readonly IGameService _gameService;
    
    public string Name => "random";
    
    public RandomCommand(
        IGameService gameService
        )
    {
        _gameService = gameService;
    }

    public async Task ExecuteAsync(SocketMessage message, string[] args)
    {
        int count = 3;

        if (args.Length > 0 && int.TryParse(args[0], out var parsed))
        {
            count = parsed;
        }
        
        var games = await _gameService.GetRandomGames(count);

        var response = string.Join("\n", games.Select(x => x.Name));
        
        await message.Channel.SendMessageAsync(response);
    }
}