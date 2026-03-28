using Discord.WebSocket;
using DiscordBot.Services.Interfaces;

namespace DiscordBot.Commands;

public class FindByIdCommand : ICommand
{
    private readonly IGameService _gameService;
    
    public string Name => "findbyid";
    
    public  FindByIdCommand(IGameService gameService)
    {
        _gameService = gameService;
    }

    public async Task ExecuteAsync(SocketMessage message, string[] args)
    {
        if (args.Length == 0 || !int.TryParse(args[0], out var id))
        {
            await message.Channel.SendMessageAsync("Please provide game with valid ID");
            return;
        }
        
        var game = await _gameService.FindGameByIdAsync(id);

        if (game is null)
        {
            await message.Channel.SendMessageAsync("Game not found");
            return;
        }

        var response = $"Id: {game.Id}\nName: {game.Name}\nTags: {game.Tags}";
        
        await message.Channel.SendMessageAsync(response);
    }
}