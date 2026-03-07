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
        Task<Guid> CreateGenreAsync(CreateGenreRequest request, CancellationToken token=default);
        Task<bool> UpdateGenreAsync(UpdateGenreRequest request, CancellationToken token= default);
        Task<IEnumerable<GameDto>> GetGameByGenreAsync(Guid id, CancellationToken token = default);
        Task<GenreDto> GetGenreByIdAsync(Guid id, CancellationToken token = default);
        Task<bool> DeleteGenreAsync(Guid id, CancellationToken token = default);
        Task<IEnumerable<GenreDto>> GetGenresByParentIdAsync(Guid id, CancellationToken token = default);
        Task<IEnumerable<GenreDto>> GetAllGenresAsync(CancellationToken token = default);
    }
}
