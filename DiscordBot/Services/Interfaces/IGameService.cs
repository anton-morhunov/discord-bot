using DiscordBot.Models;

namespace DiscordBot.Services.Interfaces;

public interface IGameService
{
    GameModel? GetRandomGames();
   // List<GameModel> FindGameByTags(List<string> args);
   Task<List<GameModel>> GetGameByTagsAsync(List<string> tags);
}