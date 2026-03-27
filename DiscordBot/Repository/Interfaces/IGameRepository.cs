using DiscordBot.Models;

namespace DiscordBot.Repository.Interfaces;

public interface IGameRepository
{
    Task<List<GameModel>> FindByTagsAsync(List<string> tags);
    Task<List<GameModel>> GetAllGamesAsync();
    Task<GameModel> AddGameAsync(GameModel game);
}