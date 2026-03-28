using Discord;
using DiscordBot.Dto;
using DiscordBot.Models;

namespace DiscordBot.Services.Interfaces;

public interface IGameService
{ 
   Task<List<GameModel>> GetRandomGames(int count);
   Task<List<GameModel>> GetGameByTagsAsync(List<string> tags);
   Task<GameModel> AddGameAsync(string[] arg);
   Task<GamesResponseDto?> FindGameByIdAsync(int id);
   Task DeleteGameByIdAsync(int id);
}