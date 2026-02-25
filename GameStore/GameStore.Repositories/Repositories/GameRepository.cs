using GameStore.Repositories.Interfaces;
using GameStore.Infrastructure.Data;
using GameStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameStore.Repositories.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly GameStoreContext dbContext;
        public GameRepository(GameStoreContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task AddGameAsync(Game game,CancellationToken token)
        {
            await dbContext.Games.AddAsync(game, token);
            await dbContext.SaveChangesAsync(token);
        }
        public async Task DeleteGameAsync(Game game,CancellationToken token)
        {
            if (game != null)
            {
                dbContext.Games.Remove(game);
                await dbContext.SaveChangesAsync(token);
            }
        }

        public async Task<IEnumerable<Game>> GetAllGamesAsync(CancellationToken token)
        {
            return await dbContext.Games
                .Include(g => g.GameGenres).ThenInclude(gg => gg.Genre)
                .Include(g => g.GamePlatforms).ThenInclude(gp => gp.Platform)
                .ToListAsync(token);

        }
        public async Task<IEnumerable<Genre>> GetGameGenresByKeyAsync(string key, CancellationToken token)
        {
            return await dbContext.Games
                .Where(g => g.Key == key)
                .SelectMany(g => g.GameGenres)
                .Select(gg => gg.Genre)
                .ToListAsync(token);
        }
        public async Task<IEnumerable<Platform>> GetGamePlatformsByKeyAsync(string key, CancellationToken token)
        {
            return await dbContext.Games
                .Where(g => g.Key == key)
                .SelectMany(g => g.GamePlatforms)
                .Select(gp => gp.Platform)
                .ToListAsync(token);
        }
        public async Task<Game>? GetGameByKeyAsync(string key,CancellationToken token)
        {
            return await dbContext.Games
                .Include(g => g.GameGenres).ThenInclude(gg => gg.Genre)
                .Include(g => g.GamePlatforms).ThenInclude(gp => gp.Platform)
                .FirstOrDefaultAsync(g => g.Key == key,token);
        }
        public async Task<Game> GetGameByIdAsync(Guid id,CancellationToken token)
        {
            return await dbContext.Games
                .Include(g => g.GameGenres).ThenInclude(gg => gg.Genre)
                .Include(g => g.GamePlatforms).ThenInclude(gp => gp.Platform)
                .FirstOrDefaultAsync(g => g.Id == id, token);
        }
    }
}
