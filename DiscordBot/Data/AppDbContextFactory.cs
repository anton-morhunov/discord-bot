using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace DiscordBot.Data;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
        
        var connectionString = config.GetConnectionString("DefaultConnection");

        optionsBuilder.UseNpgsql(connectionString);
        
        return new AppDbContext(optionsBuilder.Options);
    }
}