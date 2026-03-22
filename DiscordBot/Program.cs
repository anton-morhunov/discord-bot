using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using DiscordBot;
using DiscordBot.Commands;
using DiscordBot.Handlers;
using DiscordBot.Services;
using Microsoft.Extensions.Configuration;

var services = new ServiceCollection();

services.AddSingleton<GameService>();
services.AddSingleton<BotService>();
services.AddSingleton<DiscordSocketClient>(provider =>
{
    var config = new DiscordSocketConfig
    {
        GatewayIntents = GatewayIntents.Guilds
                         | GatewayIntents.GuildMessages
                         | GatewayIntents.MessageContent
    };
    
    return new DiscordSocketClient(config);
});
services.AddSingleton<CommandHandler>();

services.AddSingleton<ICommand, GameCommand>();
services.AddSingleton<ICommand, HelpCommand>();
services.AddSingleton<ICommand, RandomCommand>();

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true)
    .AddUserSecrets(typeof(Program).Assembly)
    .AddEnvironmentVariables()
    .Build();

services.AddSingleton<IConfiguration>(config);

var provider =  services.BuildServiceProvider();
var bot = provider.GetRequiredService<BotService>();

await bot.StartAsync();
await Task.Delay(-1);