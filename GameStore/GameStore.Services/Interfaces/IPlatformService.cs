using System;
using GameStore.Application.DTO;
using System.Collections.Generic;
using System.Text;
using GameStore.Application.Requests;

namespace GameStore.Services.Interfaces
{
    public interface IPlatformService
    {
        Task<PlatformDto> CreatePlatformAsync(CreatePlatformRequest request, CancellationToken token=default);
        Task<bool> UpdatePlatformAsync(UpdatePlatformRequest request, CancellationToken token=default);
        Task<bool> DeletePlatformAsync(Guid id, CancellationToken token = default);
        Task<PlatformDto> GetPlatformByIdAsync(Guid id, CancellationToken token = default);
        Task<IEnumerable<PlatformDto>> GetAllPlatformsAsync(CancellationToken token=default);
        Task<IEnumerable<GameDto>> GetGameByPlatformAsync(Guid id, CancellationToken token = default);
    }
}
