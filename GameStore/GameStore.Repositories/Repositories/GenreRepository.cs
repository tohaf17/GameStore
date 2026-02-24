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
        public async Task AddGenreAsync(Genre genre,CancellationToken token)
        {
            await dbContext.Genres.AddAsync(genre, token);
            await dbContext.SaveChangesAsync(token);
        } 
        public async Task<bool> UpdateGenreAsync(Genre genre, CancellationToken token)
        {
            dbContext.Genres.Update(genre);
            await dbContext.SaveChangesAsync(token);
            return true;
        }
        public async Task<Genre> GetGenreByIdAsync(Guid id, CancellationToken token)
        {
            return await dbContext.Genres.FindAsync(new object[] { id }, token);
        }
        public async Task<IEnumerable<Genre>> GetAllGenresAsync(CancellationToken token)
        {
            return await dbContext.Genres.ToListAsync(token);
        }
        public async Task<IEnumerable<Genre>> GetGenresByParentIdAsync(Guid id, CancellationToken token)
            {
                return await dbContext.Genres.Where(g => g.ParentGenreId == id).ToListAsync(token);
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
