using GameStore.Application.Requests;
using GameStore.Application.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameStore.Services.Interfaces
{
    public interface IGameService
    {
        Task<Guid> CreateAsync(CreateGameRequest request, CancellationToken token);
        Task<GameDTO> GetGameByKeyAsync(string key, CancellationToken token);
    }
}
