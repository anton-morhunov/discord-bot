using Discord.WebSocket;

namespace DiscordBot;

public interface ICommand
{
    string Name { get; }
    Task ExecuteAsync(SocketMessage message, string[] args);
}