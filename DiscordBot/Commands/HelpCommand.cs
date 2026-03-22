using Discord.WebSocket;
using DiscordBot.Handlers;
using DiscordBot.Services;

namespace DiscordBot.Commands;

public class HelpCommand : ICommand
{
    private readonly GameService _gameService;
    
    public string Name => "help";

    public HelpCommand(
        GameService gameService
    )
    {
        _gameService = gameService;
    }

    public async Task ExecuteAsync(SocketMessage message, string[] args)
    {
        await message.Channel.SendMessageAsync(
            "Avaliable commands: \n!game <tags>\n!help");
    }
}