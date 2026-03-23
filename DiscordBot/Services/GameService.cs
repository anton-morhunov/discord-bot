using DiscordBot.Models;
using DiscordBot.Services.Interfaces;

namespace DiscordBot.Services;

public class GameService : IGameService
{
    public List<GameModel> _games = new List<GameModel>();
    
    public GameModel? GetRandomGames()
    {
        if (_games.Count == 0)
            return null;
        
        var random = new Random();
        var index = random.Next(_games.Count);
        
        return _games[index];
    }

    public List<GameModel> FindGameByTags(List<string> args)
    {
        var result = new List<GameModel>();
        var temp = new List<(GameModel game, int score)>();

        foreach (var game in _games)
        {
            var score = 0;
            
            foreach (var arg  in args)
            {
                var lowerArg = arg.ToLower();
                if (game.Tags.Contains(lowerArg))
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
}