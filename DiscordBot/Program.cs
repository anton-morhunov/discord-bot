using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using DiscordBot;
using DiscordBot.Commands;
using DiscordBot.Data;
using DiscordBot.Handlers;
using DiscordBot.Repository;
using DiscordBot.Repository.Interfaces;
using DiscordBot.Services;
using DiscordBot.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var services = new ServiceCollection();

services.AddSingleton<IGameService, GameService>();
services.AddSingleton<IGameRepository, EfGameRepository>();
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
services.AddSingleton<ICommand, AddGameCommand>();

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true)
    .AddUserSecrets(typeof(Program).Assembly)
    .AddEnvironmentVariables()
    .Build();

services.AddDbContext<AppDbContext>(options => 
    options.UseNpgsql(config.GetConnectionString("DefaultConnection")));

services.AddSingleton<IConfiguration>(config);

var provider =  services.BuildServiceProvider();
var bot = provider.GetRequiredService<BotService>();

using (var scope = provider.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    DbSeeder.Seed(context);
}

await bot.StartAsync();
await Task.Delay(-1);