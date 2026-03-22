using Discord.WebSocket;
using DiscordBot.Handlers;
using DiscordBot.Services;

namespace DiscordBot.Commands;

public class RandomCommand : ICommand
{
    private readonly GameService _gameService;
    
    public string Name => "random";
    
    public RandomCommand(
        GameService gameService
        )
    {
        _gameService = gameService;
    }

    public async Task ExecuteAsync(SocketMessage message, string[] args)
    {
        var games = _gameService.GetRandomGames();

        if (games == null)
        {
            await message.Channel.SendMessageAsync("No games found.");
        }

        await message.Channel.SendMessageAsync($"Random game {games.Name}");
    }
}