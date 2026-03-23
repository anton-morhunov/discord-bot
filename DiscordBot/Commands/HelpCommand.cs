using Discord.WebSocket;
using DiscordBot.Handlers;
using DiscordBot.Services;
using DiscordBot.Services.Interfaces;

namespace DiscordBot.Commands;

public class HelpCommand : ICommand
{
    private readonly IGameService _gameService;
    
    public string Name => "help";

    public HelpCommand(
        IGameService gameService
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