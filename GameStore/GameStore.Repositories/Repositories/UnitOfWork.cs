using GameStore.Infrastructure.Data;
using GameStore.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Repositories.Repositories
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly GameStoreContext context;
        private bool disposed = false;
        public IGameRepository Games{ get; set; }
        public IGenreRepository Genres { get; set; }
        public IPlatformRepository Platforms { get; set; }
        public UnitOfWork(GameStoreContext context, IGameRepository games,IGenreRepository genres,IPlatformRepository platforms)
        {
            this.context = context;
            Games = games;
            Genres = genres;
            Platforms = platforms;
        }

        public async Task SaveChangesAsync(CancellationToken token=default)
        {
            await context.SaveChangesAsync(token);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

