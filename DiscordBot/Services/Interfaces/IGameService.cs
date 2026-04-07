using Discord;
using DiscordBot.Dto;
using DiscordBot.Models;

namespace DiscordBot.Services.Interfaces;

public interface IGameService
{ 
   Task<List<GamesResponseDto>> GetRandomGames(int count);
   Task<List<GamesResponseDto>> GetGameByTagsAsync(List<string> tags);
   Task<GamesResponseDto> AddGameAsync(string[] arg);
   Task<GamesResponseDto?> FindGameByIdAsync(int id);
   Task DeleteGameByIdAsync(int id);
}