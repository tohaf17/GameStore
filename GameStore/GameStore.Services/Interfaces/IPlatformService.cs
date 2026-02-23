using System;
using GameStore.Application.DTO;
using System.Collections.Generic;
using System.Text;

namespace GameStore.Services.Interfaces
{
    public interface IPlatformService
    {
        Task<IEnumerable<GameDTO>> GetGameByPlatformAsync(Guid id, CancellationToken token);
    }
}
