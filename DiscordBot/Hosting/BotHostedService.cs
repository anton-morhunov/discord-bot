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
        
        context.Database.Migrate();
        
        await _botService.StartAsync(cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _botService.StopAsync(cancellationToken);
    }
}