using Discord.WebSocket;
using DiscordBot.Services;

namespace DiscordBot.Commands;

public class GameCommand : ICommand
{
    private readonly GameService _gameService;

    public string Name => "game";
    
    public GameCommand(
        GameService gameService
        )
    {
        _gameService = gameService;
    }
    
    public async Task ExecuteAsync(SocketMessage message, string[] args)
    {
        var games = _gameService.GetGames(args.ToList());

        if (games.Count == 0)
        {
            await message.Channel.SendMessageAsync("No games found.");
            return;
        }

        string response = "Recommended games: \n";

        foreach (var game in games)
        {
            response += $"\n- {game.Name} " +
                        string.Join(", ", game.Tags);
        }
        
        await message.Channel.SendMessageAsync(response);
    }
}