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
        Task<bool> DeletePlatformAsync(Guid id, CancellationToken token);
        Task<PlatformDto> GetPlatformByIdAsync(Guid id, CancellationToken token);
        Task<IEnumerable<PlatformDto>> GetAllPlatformsAsync(CancellationToken token);
        Task<IEnumerable<GameDto>> GetGameByPlatformAsync(Guid id, CancellationToken token);
    }
}
