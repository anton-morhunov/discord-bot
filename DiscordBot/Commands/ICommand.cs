using Discord.WebSocket;

namespace DiscordBot.Commands;

public interface ICommand
{
    string Name { get; }
    Task ExecuteAsync(SocketMessage message, string[] args);
}