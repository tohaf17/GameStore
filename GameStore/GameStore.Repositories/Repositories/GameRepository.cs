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

        public async Task AddAsync(Game game,CancellationToken token)
        {
            await dbContext.Games.AddAsync(game, token);
            await dbContext.SaveChangesAsync(token);
        }
        public async Task<Game> GetGameByKeyAsync(string key,CancellationToken token)
        {
            return await dbContext.Games
                .Include(g => g.GameGenres).ThenInclude(gg => gg.Genre)
                .Include(g => g.GamePlatforms).ThenInclude(gp => gp.Platform)
                .FirstOrDefaultAsync(g => g.Key == key,token);
        }
    }
}
