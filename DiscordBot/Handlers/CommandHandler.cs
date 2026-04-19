using Discord.WebSocket;
using DiscordBot.Commands;

namespace DiscordBot.Handlers;
public class CommandHandler
{
    private Dictionary<string, ICommand> _commandHandlers;

    public CommandHandler(IEnumerable<ICommand> commands)
    {
        _commandHandlers = commands.ToDictionary(x => x.Name);
    }
    public async Task HandleMessageAsync(SocketMessage socketMessage)
    {
        if (socketMessage.Author.IsBot) return;
        
        var content = socketMessage.Content.Trim();
        
        if (!content.StartsWith("/")) return;

        var arg =  content.Substring(1).ToLower();
        var parts = arg.Split(' ');
        
        var command = parts[0].ToLower();
        var tags = parts.Skip(1).ToArray();

        if (_commandHandlers.TryGetValue(command, out var handler))
        {
            await handler.ExecuteAsync(socketMessage, tags);
        }

        else
        {
            await socketMessage.Channel.SendMessageAsync("Invalid command.");
        }
    }
}