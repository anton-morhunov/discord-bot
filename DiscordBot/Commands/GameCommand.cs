using Discord.WebSocket;
using DiscordBot.Services.Interfaces;

namespace DiscordBot.Commands;

public class GameCommand : ICommand
{
    private readonly IGameService _gameService;

    public string Name => "game";
    
    public GameCommand(
        IGameService gameService
        )
    {
        _gameService = gameService;
    }
    
    public async Task ExecuteAsync(SocketMessage message, string[] args)
    {
        var tags = args.Select(a => a.ToLower()).ToList();

        var games = await _gameService.GetGameByTagsAsync(tags);

        if (!games.Any())
        {
            await message.Channel.SendMessageAsync("No games found.");
            return;
        }

        var response = string.Join("\n", games.Select(g => g.Name));

        await message.Channel.SendMessageAsync(response);
    }
    
    
}