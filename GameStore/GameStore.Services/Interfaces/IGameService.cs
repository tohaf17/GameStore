using GameStore.Application.Requests;
using GameStore.Application.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameStore.Services.Interfaces
{
    public interface IGameService
    {
        Task<Guid> CreateGameAsync(CreateGameRequest request, CancellationToken token);
        Task<bool> UpdateGameAsync(UpdateGameRequest request, CancellationToken token);
        Task<bool> DeleteGameAsync(Guid id, CancellationToken token);
        Task<IEnumerable<GameDto>> GetAllGamesAsync(CancellationToken token);
        Task<GameDto?> GetGameByKeyAsync(string key, CancellationToken token);
        Task<IEnumerable<GenreDto>> GetGameGenresByKeyAsync(string key, CancellationToken token);
        Task<IEnumerable<PlatformDto>> GetGamePlatformsByKeyAsync(string key, CancellationToken token);
        Task<GameDto> GetGameByIdAsync(Guid id, CancellationToken token);
        Task<bool> GetGameFilesAsync(Guid id, CancellationToken token);
    }
}
