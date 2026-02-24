using System;
using System.Collections.Generic;
using System.Text;
using GameStore.Domain.Entities;

namespace GameStore.Repositories.Interfaces
{
    public interface IGenreRepository
    {
        Task AddGenreAsync(Genre genre, CancellationToken token);
        Task<IEnumerable<Game>> GetGameByGenreAsync(Guid id, CancellationToken token);
        Task<Genre> GetGenreByIdAsync(Guid id, CancellationToken token);
        Task<IEnumerable<Genre>> GetAllGenresAsync(CancellationToken token);
        Task<bool> UpdateGenreAsync(Genre genre, CancellationToken token);
        Task<IEnumerable<Genre>> GetGenresByParentIdAsync(Guid id, CancellationToken token);
    }
}
