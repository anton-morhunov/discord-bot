using Discord.WebSocket;
using DiscordBot.Services.Interfaces;

namespace DiscordBot.Commands;

public class DeleteCommand : ICommand
{
    private readonly IGameService _gameService;
    
    public string Name => "delete";
    
    public DeleteCommand(
        IGameService gameService
        )
    {
        _gameService = gameService;
    }

    public async Task ExecuteAsync(SocketMessage socketMessage, string[] args)
    {
        int id = 0;

        if (args.Length > 0 && int.TryParse(args[0], out var gameId))
        {
            id = gameId;
        }
        
        await _gameService.DeleteGameByIdAsync(id);
        
        await socketMessage.Channel.SendMessageAsync($"Deleted game with id {id}");
    }
}