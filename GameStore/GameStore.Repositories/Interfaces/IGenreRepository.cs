using System;
using System.Collections.Generic;
using System.Text;
using GameStore.Domain.Entities;

namespace GameStore.Repositories.Interfaces
{
    public interface IGenreRepository
    {
        Task AddGenreAsync(Genre genre);
        Task UpdateGenreAsync(Genre genre);
        Task DeleteGenreAsync(Genre genre);
        Task<IEnumerable<Game>> GetGameByGenreAsync(Guid id, CancellationToken token=default);
        Task<Genre?> GetGenreByIdAsync(Guid id, CancellationToken token=default);
        Task<IEnumerable<Genre>> GetAllGenresAsync(CancellationToken token=default);
        Task<IEnumerable<Genre>> GetGenresByParentIdAsync(Guid id, CancellationToken token= default);
    }
}
