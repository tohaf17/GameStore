using System;
using System.Collections.Generic;
using GameStore.Application.Requests;
using GameStore.Domain.Entities;   

using GameStore.Application.DTO;
using System.Text;

namespace GameStore.Services.Interfaces
{
    public interface IGenreService
    {
        Task<Guid> CreateGenreAsync(CreateGenreRequest request, CancellationToken token);
        Task<IEnumerable<GameDTO>> GetGameByGenreAsync(Guid id, CancellationToken token);
        Task<GenreDTO> GetGenreByIdAsync(Guid id, CancellationToken token);
            Task<IEnumerable<GenreDTO>> GetGenresByParentIdAsync(Guid id, CancellationToken token);
        Task<IEnumerable<GenreDTO>> GetAllGenresAsync(CancellationToken token);
    }
}
