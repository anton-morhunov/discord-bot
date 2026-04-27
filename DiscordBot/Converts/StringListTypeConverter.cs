using Discord;
using Discord.Interactions;

namespace DiscordBot.Converts;

public class StringListTypeConverter : TypeConverter<List<string>>
{
    public override ApplicationCommandOptionType GetDiscordType() 
        => ApplicationCommandOptionType.String;

    public override Task<TypeConverterResult> ReadAsync(
        IInteractionContext context, 
        IApplicationCommandInteractionDataOption option,
        IServiceProvider services)
    {
        var input = option.Value.ToString();
        
        var list = input.Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Trim())
            .ToList();
        
        return Task.FromResult(TypeConverterResult.FromSuccess(list));
    }
}
