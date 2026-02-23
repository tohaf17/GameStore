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
        Task<IEnumerable<GameDTO>> GetAllGamesAsync(CancellationToken token);
        Task<GameDTO> GetGameByKeyAsync(string key, CancellationToken token);
        Task<GameDTO> GetGameByIdAsync(Guid id, CancellationToken token);
        Task<bool> GetGameFilesAsync(Guid id, CancellationToken token);
    }
}
