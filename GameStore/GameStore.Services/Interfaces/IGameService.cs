using GameStore.Application.Requests;
using GameStore.Application.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameStore.Services.Interfaces
{
    public interface IGameService
    {
        Task<GameDto> CreateGameAsync(CreateGameRequest request, CancellationToken token=default);
        Task<bool> UpdateGameAsync(UpdateGameRequest request, CancellationToken token=default);
        Task<bool> DeleteGameAsync(Guid id, CancellationToken token = default);
        Task<IEnumerable<GameDto>> GetAllGamesAsync(CancellationToken token=default);
        Task<GameDto?> GetGameByKeyAsync(string key, CancellationToken token = default);
        Task<IEnumerable<GenreDto>> GetGameGenresByKeyAsync(string key, CancellationToken token = default);
        Task<IEnumerable<PlatformDto>> GetGamePlatformsByKeyAsync(string key, CancellationToken token = default);
        Task<GameDto> GetGameByIdAsync(Guid id, CancellationToken token = default);
        Task<bool> GetGameFilesAsync(Guid id, CancellationToken token = default);
    }
}
