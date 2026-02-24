using System;
using GameStore.Application.DTO;
using System.Collections.Generic;
using System.Text;
using GameStore.Application.Requests;

namespace GameStore.Services.Interfaces
{
    public interface IPlatformService
    {
        Task<Guid> CreatePlatformAsync(CreatePlatformRequest request, CancellationToken token);
        Task<bool> UpdatePlatformAsync(UpdatePlatformRequest request, CancellationToken token);
        Task<PlatformDTO> GetPlatformByIdAsync(Guid id, CancellationToken token);
        Task<IEnumerable<PlatformDTO>> GetAllPlatformsAsync(CancellationToken token);
        Task<IEnumerable<GameDTO>> GetGameByPlatformAsync(Guid id, CancellationToken token);
    }
}
