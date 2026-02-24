using System;
using GameStore.Repositories.Interfaces;
using GameStore.Infrastructure.Data;
using GameStore.Domain.Entities;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Repositories.Repositories
{
    public class PlatformRepository : IPlatformRepository
    {
        private readonly GameStoreContext dbContext;
        public PlatformRepository(GameStoreContext dbContext)
        {
            this.dbContext=dbContext;
        }

        public async Task AddPlatformAsync(Platform platform, CancellationToken token)
        {
            await dbContext.Platforms.AddAsync(platform, token);
            await dbContext.SaveChangesAsync(token);
        }
        public async Task<IEnumerable<Game>> GetGameByPlatformAsync(Guid id, CancellationToken token)
        {
            return await dbContext.Games
                .Include(g => g.GameGenres).ThenInclude(gg => gg.Genre)
                .Include(g=>g.GamePlatforms).ThenInclude(gp=>gp.Platform)
                .Where(g=>g.GamePlatforms.Any(gp=>gp.PlatformId==id)).ToListAsync();
        }
    }
}
