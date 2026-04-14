using DiscordBot.Dto;
using DiscordBot.Models;
using DiscordBot.Repository.Interfaces;
using DiscordBot.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace DiscordBot.Services;

public class GameService : IGameService
{
    private readonly IGameRepository _gameRepository;
    private readonly ILogger<GameService> _logger;

    public GameService(
        IGameRepository gameRepository,
        ILogger<GameService> logger
        )
    {
        _gameRepository = gameRepository;
        _logger = logger;
    }

    public async Task<List<GamesResponseDto>> GetRandomGames(int count)
    {
        var games = await _gameRepository.GetAllGamesAsync();
        
        _logger.LogInformation(
            "Getting random {count} games", 
            count
            );
        
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
        
        _logger.LogInformation(
            "Getting games by tags {@tags}", 
            tags
            );

        var result = new List<GameModel>();
        var temp = new List<(GameModel game, int score)>();

        
        foreach (var game in getGames)
        {
            var score = 0;
            
            var gameTags = game.Tags
                .Split(',')
                .Select(x => x.Trim().ToLower())
                .ToList();

            foreach (var tag in tags)
            {
                var normalizedTags = tag
                    .Trim()
                    .ToLower();
                
                if (gameTags.Contains(normalizedTags))
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
        
        _logger.LogInformation(
            "Selected top {Count} games by tags {@tags}", 
            3, 
            tags
            );
        
        return result.Select(game => new GamesResponseDto
        {
            Name = game.Name,
            Tags = game.Tags
        }).ToList();
    }

    public async Task<GamesResponseDto> AddGameAsync(string[] arg)
    {
        _logger.LogDebug(
            "Adding game {@arg}", 
            arg
            );

        if (arg.Length == 0)
        {
            _logger.LogWarning("No argument provided");
            throw new ArgumentException("Arguments is required");
        }
        
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
        
        _logger.LogInformation(
            "Added a new game with " +
            "Name: {Name}\n" +
            "Tags: {Tags}", 
            createGame.Name, 
            createGame.Tags
            );
        
        return response;
    }

    public async Task<GamesResponseDto?> FindGameByIdAsync(int id)
    {
        _logger.LogDebug(
            "Finding a game by {id}", 
            id
            );
        
        var game = await _gameRepository.FindGameByIdAsync(id);

        if (game == null)
        {
            _logger.LogWarning(
                "Game with id {id} not found", 
                id
                );
            
            return null;
        }

        _logger.LogInformation(
            "Found a game with id {id}", 
            game.Id);
        
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
            _logger.LogWarning(
                "Deleting failed. No game found with id {id}", 
                id
                );
            
            throw new Exception("Game not found");
        }
        
        _logger.LogInformation(
            "Deleting game with id {id}", 
            id
            );
        
        await _gameRepository.DeleteGameAsync(id);
    }
}