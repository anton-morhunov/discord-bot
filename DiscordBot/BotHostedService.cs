using DiscordBot.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DiscordBot;

public class BotHostedService : IHostedService
{
    private readonly BotService _botService;
    private readonly IServiceProvider _serviceProvider;

    public BotHostedService(
        BotService botService,
        IServiceProvider serviceProvider
    )
    {
        _botService = botService;
        _serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        DbSeeder.Seed(context);
        
        await _botService.StartAsync();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}