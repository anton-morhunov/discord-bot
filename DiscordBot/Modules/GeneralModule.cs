using Discord.Interactions;
using DiscordBot.Services.Interfaces;

namespace DiscordBot.Modules;

public class GeneralModule : InteractionModuleBase<SocketInteractionContext>
{
    private readonly IGameService _gameService;

    public GeneralModule(IGameService gameService)
    {
        _gameService = gameService;
    }
    
    [SlashCommand("help", "Displays help")]
    public async Task Help()
    {
        await RespondAsync("Avaliable commands: \n/game <tags>\n/help\n/addgame\n/delete");
    }

    [SlashCommand("find", "Find a game by tags")]
    public async Task FindGame(string tags)
    {
        var list = tags.Split(',',  StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Trim())
            .ToList();
        
        var games = await _gameService.GetGameByTagsAsync(list);
        
        if (!games.Any())
        {
            await RespondAsync("No game found");
        }

        var response = string.Join("\n", games.Select(g => g.Name));
        
        await RespondAsync(response);
    }

    [SlashCommand("add", "Add a game")]
    public async Task AddGame(string name, string tags)
    {
             var list = tags.Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Trim())
            .ToList();
             
             var args = new[] {name}.Concat(list).ToArray();
             var result = _gameService.AddGameAsync(args);
             
             await RespondAsync($"Added {name} with tags: {string.Join(", ", args.Skip(1))}");
    }
}