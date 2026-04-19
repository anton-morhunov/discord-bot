using Discord.Interactions;

namespace DiscordBot.Modules;

public class GeneralModule : InteractionModuleBase<SocketInteractionContext>
{
    [SlashCommand("help", "Displays help")]
    public async Task Help()
    {
        await RespondAsync("Avaliable commands: \n/game <tags>\n/help\n/addgame\n/delete");
    }
}