using Discord.WebSocket;

namespace DiscordBot.Commands;

public class HelpCommand : ICommand
{
    public string Name => "help";

    public async Task ExecuteAsync(SocketMessage message, string[] args)
    {
        await message.Channel.SendMessageAsync(
            "Avaliable commands: \n/game <tags>\n/help\n/addgame\n/delete");
    }
}