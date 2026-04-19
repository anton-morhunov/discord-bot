using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using DiscordBot.Commands;
using DiscordBot.Data;
using DiscordBot.Handlers;
using DiscordBot.Hosting;
using DiscordBot.Repository;
using DiscordBot.Repository.Interfaces;
using DiscordBot.Services;
using DiscordBot.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

var host = Host.CreateDefaultBuilder(args)
    .UseSerilog((context, services, configuration) =>
        {
            configuration
                .ReadFrom.Configuration(context.Configuration)
                .WriteTo.Console()
                .WriteTo.File("logs/discord-bot.log", rollingInterval: RollingInterval.Day);
        }
    ).ConfigureServices((context, services) =>
    {
        var configuration = context.Configuration;
        
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
        services.AddSingleton<ICommand, DeleteCommand>();
        services.AddSingleton<ICommand, FindByIdCommand>();
        services.AddSingleton(x =>
        {
            var client = x.GetRequiredService<DiscordSocketClient>();
            return new InteractionService(client.Rest);
        });

        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true)
            .AddUserSecrets(typeof(Program).Assembly)
            .AddEnvironmentVariables()
            .Build();

        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(config.GetConnectionString("DefaultConnection")));

        services.AddSingleton<IConfiguration>(config);
        services.AddHostedService<BotHostedService>();
    }).Build();
    
await host.RunAsync();