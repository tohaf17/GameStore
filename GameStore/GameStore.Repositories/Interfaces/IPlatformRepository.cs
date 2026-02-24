using System;
using System.Collections.Generic;
using GameStore.Domain.Entities;
using System.Text;

namespace GameStore.Repositories.Interfaces
{
    public interface IPlatformRepository
    {
        Task AddPlatformAsync(Platform platform, CancellationToken token);
        Task<IEnumerable<Platform>> GetAllPlatformsAsync(CancellationToken token);
        Task<Platform> GetPlatformByIdAsync(Guid id,CancellationToken token);
        Task<IEnumerable<Game>> GetGameByPlatformAsync(Guid id, CancellationToken token);
    }
}
