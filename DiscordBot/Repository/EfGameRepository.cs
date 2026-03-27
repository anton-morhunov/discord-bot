using DiscordBot.Data;
using DiscordBot.Models;
using DiscordBot.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DiscordBot.Repository;

public class EfGameRepository : IGameRepository
{
    private readonly AppDbContext _db;
    
    public  EfGameRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<GameModel>> FindByTagsAsync(List<string> tags)
    {
        var query = _db.Games.AsQueryable();

        foreach (var tag in tags)
        {
            var toLower = tag.ToLower();
            query = query
                .Where(g => g.Tags.ToLower()
                .Contains(toLower));
        }

        return await query.ToListAsync();
    }

    public async Task<List<GameModel>> GetAllGamesAsync()
    {
        return await _db.Games
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<GameModel> AddGameAsync(GameModel game)
    {
        await _db.Games.AddAsync(game);
        _db.SaveChanges();

        return game;
    }
}