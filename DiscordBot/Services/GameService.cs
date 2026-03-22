using Discord;
using DiscordBot.Models;

namespace DiscordBot.Services;

public class GameService
{
    public List<GameModel> _games = new List<GameModel>();

    public GameService()
    {
        _games.Add(new GameModel
        {
            Name = "Elden Ring",
            Tags = new List<string>{"rpg", "open-world", "single-player"}
        });

        _games.Add(new GameModel()
        {
            Name = "Baldurs Gate",
            Tags = new List<string>{"rpg", "story", "coop"}
        });
        
        _games.Add(new GameModel()
        {
            Name = "The Witcher 3",
            Tags = new List<string>{"rpg", "open-world", "story"}
        });
        
        _games.Add(new GameModel()
        {
            Name = "Cyberpunk 2077",
            Tags = new List<string>{"rpg", "open-world", "futuristic"}
        });
        
        _games.Add(new GameModel()
        {
            Name = "Dark Souls 3",
            Tags = new List<string>{"rpg", "hardcore", "single-player"}
        });
        
        _games.Add(new GameModel()
        {
            Name = "Counter-Strike 2",
            Tags = new List<string>{"fps", "multiplayer", "competitive"}
        });
        
        _games.Add(new GameModel()
        {
            Name = "Call of Duty Warzone",
            Tags = new List<string>{"fps", "multiplayer", "battle-royale"}
        });
        
        _games.Add(new GameModel()
        {
            Name = "Apex Legends",
            Tags = new List<string>{"fps", "battle-royale", "multiplayer"}
        });
        
        _games.Add(new GameModel()
        {
            Name = "Valorant",
            Tags = new List<string>{"fps", "competitive", "tactical"}
        });
        
        _games.Add(new GameModel()
        {
            Name = "Minecraft",
            Tags = new List<string>{"sandbox", "survival", "creative"}
        });
    }

    public GameModel? GetRandomGames()
    {
        if (_games.Count == 0)
            return null;
        
        var random = new Random();
        var index = random.Next(_games.Count);
        
        return _games[index];
    }

    public List<GameModel> GetGames(List<string> args)
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