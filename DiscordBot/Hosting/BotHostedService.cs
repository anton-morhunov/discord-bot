using Discord.WebSocket;
using DiscordBot.Data;
using DiscordBot.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DiscordBot.Hosting;

public class BotHostedService : IHostedService
{
    private readonly BotService _botService;
    private readonly IServiceProvider _serviceProvider;
    private readonly DiscordSocketClient  _client;

    public BotHostedService(
        BotService botService,
        IServiceProvider serviceProvider,
        DiscordSocketClient client
    )
    {
        _botService = botService;
        _serviceProvider = serviceProvider;
        _client = client;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("=== HOSTED SERVICE START ===");
        
        _client.Ready -= OnReady;
        _client.Ready += OnReady;
        
        _client.Log += msg =>
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        };
        
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        
        context.Database.Migrate();
        
        Console.WriteLine("BEFORE BOT START");

        await _botService.StartAsync(cancellationToken);
        
        Console.WriteLine("AFTER BOT START");
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _botService.StopAsync(cancellationToken);
    }

    private Task OnReady()
    {
        Console.WriteLine($"Connected as: {_client.CurrentUser}");
        return Task.CompletedTask;
    }
}