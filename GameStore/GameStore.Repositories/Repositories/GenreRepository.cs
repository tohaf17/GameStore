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
        public Task AddGenreAsync(Genre genre)
        {
            dbContext.Genres.Add(genre);
            return Task.CompletedTask;
        } 
        public Task UpdateGenreAsync(Genre genre)
        {
            dbContext.Genres.Update(genre);
            return Task.CompletedTask;
        }
        public Task DeleteGenreAsync(Genre genre)
        {
            dbContext.Genres.Remove(genre);
            return Task.CompletedTask;
        }
        public async Task<Genre?> GetGenreByIdAsync(Guid id, CancellationToken token = default)
        {
            return await dbContext.Genres.FindAsync(new object[] { id }, token);
        }
        public async Task<IEnumerable<Genre>> GetAllGenresAsync(CancellationToken token = default)
        {
            return await dbContext.Genres.ToListAsync(token);
        }
        public async Task<IEnumerable<Genre>> GetGenresByParentIdAsync(Guid id, CancellationToken token = default)
            {
                return await dbContext.Genres.Where(g => g.ParentGenreId == id).ToListAsync(token);
        }
        public async Task<IEnumerable<Game>> GetGameByGenreAsync(Guid id, CancellationToken token = default)
        {
            return await dbContext.Games
                .Include(g => g.GameGenres).ThenInclude(gg => gg.Genre)
                .Include(g => g.GamePlatforms).ThenInclude(gp => gp.Platform)
                .Where(g => g.GameGenres.Any(gg => gg.GenreId == id)).ToListAsync(token);
        }
    }
}
