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
    }
}
