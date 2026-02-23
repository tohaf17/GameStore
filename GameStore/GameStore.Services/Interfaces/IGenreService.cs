using System;
using System.Collections.Generic;
using GameStore.Application.DTO;
using System.Text;

namespace GameStore.Services.Interfaces
{
    public interface IGenreService
    {
        Task<IEnumerable<GameDTO>> GetGameByGenreAsync(Guid id, CancellationToken token);
    }
}
