using DiscordBot.Models;
using DiscordBot.Repository.Interfaces;
using DiscordBot.Services.Interfaces;

namespace DiscordBot.Services;

public class GameService : IGameService
{
    private readonly IGameRepository _gameRepository;

    public GameService(
        IGameRepository gameRepository
        )
    {
        _gameRepository = gameRepository;
    }

    public async Task<List<GameModel>> GetRandomGames(int count)
    {
        var games = await _gameRepository.GetAllGamesAsync();

        return games
            .OrderBy(x => Guid.NewGuid())
            .Take(count)
            .ToList();
    }
    public async Task<List<GameModel>> GetGameByTagsAsync(List<string> tags)
    {
        var getGames = await _gameRepository.FindByTagsAsync(tags);

        var result = new List<GameModel>();
        var temp = new List<(GameModel game, int score)>();

        foreach (var game in getGames)
        {
            var score = 0;

            foreach (var tag in tags)
            {
                if (game.Tags.Contains(tag))
                {
                    score++;
                }
            }

            if (score > 0)
            {
                temp.Add((game, score));
            }
        }

        var sorted = temp.OrderByDescending(g => g.score);
        result = sorted.Select(item => item.game).Take(3).ToList();
        
        return result;
    }

    public async Task<GameModel> AddGameAsync(string[] arg)
    {
        
        var gameName = arg[0];
        var tags = arg.Skip(1).ToList();
        
        var gameModel = new GameModel
        {
            Name = gameName,
            Tags = string.Join(", ", tags)
        };
        
        var result = await _gameRepository.AddGameAsync(gameModel);
        return result;
    }
}