using DiscordBot.Dto;
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

    public async Task<List<GamesResponseDto>> GetRandomGames(int count)
    {
        var games = await _gameRepository.GetAllGamesAsync();
        
        return games
            .OrderBy(x => Guid.NewGuid())
            .Take(count)
            .Select(x => new GamesResponseDto
            {
                Id = x.Id,
                Name = x.Name,
                Tags = x.Tags
            })
            .ToList();
    }
    public async Task<List<GamesResponseDto>> GetGameByTagsAsync(List<string> tags)
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

        return result.Select(game => new GamesResponseDto
        {
            Name = game.Name,
            Tags = game.Tags
        }).ToList();
    }

    public async Task<GamesResponseDto> AddGameAsync(string[] arg)
    {
        var gameName = arg[0];
        var tags = arg.Skip(1).ToList();

        GameModel gameModel = new GameModel
        {
            Name = gameName,
            Tags = string.Join(", ", tags)
        };
        
        var createGame = await _gameRepository.AddGameAsync(gameModel);

        var response = new GamesResponseDto
        {
            Id = createGame.Id,
            Name = createGame.Name,
            Tags = createGame.Tags,
        };
        
        return response;
    }

    public async Task<GamesResponseDto?> FindGameByIdAsync(int id)
    {
        var game = await _gameRepository.FindGameByIdAsync(id);

        if (game == null)
        {
            return null;
        }

        return new GamesResponseDto
        {
            Id = game.Id,
            Name = game.Name,
            Tags = game.Tags,
        };
    }

    public async Task DeleteGameByIdAsync(int id)
    {
        var game = await _gameRepository.FindGameByIdAsync(id);

        if (game == null)
        {
            throw new Exception("Game not found");
            return;
        }
        
        await _gameRepository.DeleteGameAsync(id);
    }
}