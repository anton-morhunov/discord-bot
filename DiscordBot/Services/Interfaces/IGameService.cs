using DiscordBot.Models;

namespace DiscordBot.Services.Interfaces;

public interface IGameService
{
    GameModel? GetRandomGames();
    List<GameModel> FindGameByTags(List<string> args);
}