using Discord;
using DiscordBot.Models;

namespace DiscordBot.Services.Interfaces;

public interface IGameService
{ 
   Task<List<GameModel>> GetRandomGames(int count);
   Task<List<GameModel>> GetGameByTagsAsync(List<string> tags);
}