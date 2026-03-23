using DiscordBot.Data;
using DiscordBot.Repository.Interfaces;

namespace DiscordBot.Repository;

public class EfGameRepository : IGameRepository
{
    private readonly AppDbContext _db;
    
    public  EfGameRepository(AppDbContext db)
    {
        _db = db;
    }
    
    
}