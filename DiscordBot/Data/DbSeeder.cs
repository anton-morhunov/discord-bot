using DiscordBot.Models;

namespace DiscordBot.Data;

public class DbSeeder
{
    public static void Seed(AppDbContext context)
    {
        if (context.Games.Any())
            return;

        context.Games.AddRange(

            new GameModel
            {
                Name = "Elden Ring",
                Tags = "rpg,open-world,single-player"
            },
            new GameModel
            {
                Name = "Baldurs Gate",
                Tags = "rpg,story,coop"
            },
            new GameModel
            {
                Name = "The Witcher 3",
                Tags = "rpg,open-world,story"
            },
            new GameModel
            {
                Name = "Cyberpunk 2077",
                Tags = "rpg,open-world,futuristic"
            }
        );
        
        context.SaveChanges();
    }
}