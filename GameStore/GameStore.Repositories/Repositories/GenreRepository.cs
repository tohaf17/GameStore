using GameStore.Domain.Entities;
using GameStore.Infrastructure.Data;
using GameStore.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameStore.Repositories.Repositories
{
    public class GenreRepository:IGenreRepository
    {
        private readonly GameStoreContext dbContext;
        public GenreRepository(GameStoreContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<Game>> GetGameByGenreAsync(Guid id, CancellationToken token)
        {
            return await dbContext.Games
                .Include(g => g.GameGenres).ThenInclude(gg => gg.Genre)
                .Include(g => g.GamePlatforms).ThenInclude(gp => gp.Platform)
                .Where(g => g.GameGenres.Any(gg => gg.GenreId == id)).ToListAsync();
        }
    }
}
