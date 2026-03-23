using DiscordBot.Models;
using Microsoft.EntityFrameworkCore;

namespace DiscordBot.Data;

public class AppDbContext : DbContext
{
    public DbSet<GameModel> Games { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : 
        base(options){}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(GameModel).Assembly);
    }
}